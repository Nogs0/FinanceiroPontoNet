using FinanceiroPontoNet.Application.Bancos;
using FinanceiroPontoNet.Application.Bancos.Dtos;
using FinanceiroPontoNet.Application.Boletos;
using FinanceiroPontoNet.Application.Boletos.Dtos;
using FinanceiroPontoNet.Domain.Bancos;
using FinanceiroPontoNet.Domain.Boletos;
using FinanceiroPontoNet.Domain.Shared.Exceptions;
using FinanceiroPontoNet.Domain.Shared.UnitOfWork;
using Moq;

namespace FinanceiroPontoNet.Tests.Application.Boletos
{
    public class BoletoServiceTest
    {
        private readonly IBoletoService _service;
        private readonly Mock<IBoletoRepository> _repositoryMock;
        private readonly Mock<IBancoService> _bancoServiceMock;
        private readonly Mock<IUnitOfWork> _uowMock;

        public BoletoServiceTest()
        {
            _repositoryMock = new Mock<IBoletoRepository>();
            _bancoServiceMock = new Mock<IBancoService>();
            _uowMock = new Mock<IUnitOfWork>();
            _service = new BoletoService(
                _repositoryMock.Object,
                _bancoServiceMock.Object,
                _uowMock.Object
            );
        }

        [Fact(DisplayName = "Create when valid input should return correct result")]
        public async Task Create_WhenValidInput_ShouldReturnCorrectResult()
        {
            //Arrange
            var createDto = new CreateBoletoDto()
            {
                NomeDoPagador = "José Augusto",
                DocumentoDoPagador = "777.356.450-74",
                NomeDoBeneficiario = "Pedro Henrique",
                DocumentoDoBeneficiario = "93.649.615/0001-24",
                Valor = 100.00m,
                DataDeVencimento = new DateTime(2025, 10, 29),
                BancoId = Guid.NewGuid(),
            };

            //Act
            var result = await _service.CreateAsync(createDto);

            //Assert
            _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Boleto>()), Times.Once());

            _uowMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once()
            );
        }

        [Theory(DisplayName = "Create when data is invalid should throw ArgumentException")]
        [InlineData(
            "",
            "777.356.450-74",
            "Pedro Henrique",
            "93.649.615/0001-24",
            "O nome do pagador é obrigatório"
        )]
        [InlineData(
            "José Augusto",
            "777.356.450-00",
            "Pedro Henrique",
            "93.649.615/0001-24",
            "O documento do pagador é inválido."
        )]
        [InlineData(
            "José Augusto",
            "777.356.450-74",
            "",
            "93.649.615/0001-24",
            "O nome do beneficiário é obrigatório."
        )]
        [InlineData(
            "José Augusto",
            "777.356.450-74",
            "Pedro Henrique",
            "93.649.615/0001-00",
            "O documento do beneficiário é inválido."
        )]
        public async Task Create_WhenDataIsInvalid_ShouldThrowArgumentException(
            string nomePagador,
            string documentoDoPagador,
            string nomeDoBeneficiario,
            string documentoDoBeneficiario,
            string expectedMessage
        )
        {
            //Arrange
            var createDto = new CreateBoletoDto()
            {
                NomeDoPagador = nomePagador,
                DocumentoDoPagador = documentoDoPagador,
                NomeDoBeneficiario = nomeDoBeneficiario,
                DocumentoDoBeneficiario = documentoDoBeneficiario,
                Valor = 100.00m,
                DataDeVencimento = new DateTime(2025, 10, 29),
                BancoId = Guid.NewGuid(),
            };

            //Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Boleto(
                    createDto.NomeDoPagador,
                    createDto.DocumentoDoPagador,
                    createDto.NomeDoBeneficiario,
                    createDto.DocumentoDoBeneficiario,
                    createDto.Valor,
                    createDto.DataDeVencimento,
                    createDto.BancoId
                )
            );

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "Get when boleto exists and not due should return correct result")]
        public async Task Get_WhenBoletoExistsAndNotDue_ShouldReturnCorrectResult()
        {
            //Arrange
            var boletoId = Guid.NewGuid();
            var boletoDb = new Boleto(
                "José Augusto",
                "777.356.450-74",
                "Pedro Henrique",
                "93.649.615/0001-24",
                100.0m,
                DateTime.Now.AddDays(10),
                Guid.NewGuid()
            );
            boletoDb.Id = boletoId;

            _repositoryMock.Setup(r => r.GetByIdAsync(boletoId)).ReturnsAsync(boletoDb);

            //Act
            var result = await _service.GetAsync(boletoId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BoletoDto>(result);
            Assert.Equal(result.Id, boletoId);
            Assert.Equal(result.Valor, boletoDb.Valor);
        }

        [Fact(DisplayName = "Get when boleto exists and is due should return correct result")]
        public async Task Get_WhenBoletoExistsAndIsDue_ShouldReturnCorrectResult()
        {
            //Arrange
            var boletoId = Guid.NewGuid();
            var banco = new BancoDto()
            {
                Id = Guid.NewGuid(),
                Nome = "Banco Teste",
                Codigo = "1",
                PercentualDeJuros = 2m,
            };

            var boletoDb = new Boleto(
                "José Augusto",
                "777.356.450-74",
                "Pedro Henrique",
                "93.649.615/0001-24",
                100.0m,
                DateTime.Now.AddDays(-10),
                banco.Id
            );
            boletoDb.Id = boletoId;

            var valorEsperado = boletoDb.Valor + (boletoDb.Valor * banco.PercentualDeJuros / 100);
            _repositoryMock.Setup(r => r.GetByIdAsync(boletoId)).ReturnsAsync(boletoDb);
            _bancoServiceMock.Setup(s => s.GetAsync(banco.Id)).ReturnsAsync(banco);

            //Act
            var result = await _service.GetAsync(boletoId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BoletoDto>(result);
            Assert.Equal(result.Id, boletoId);
            Assert.Equal(valorEsperado, result.Valor);
        }

        [Fact(DisplayName = "Get when boleto is not found")]
        public async Task Get_WhenBoletoIsNotFound_ShouldThrowNotFoundException()
        {
            //Arrange
            var boletoId = Guid.NewGuid();

            var expectedMessage =
                $"A entidade 'Boleto' com a chave: '{boletoId}' não foi encontrada.";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.GetAsync(boletoId)
            );

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "GetAll when boletos exists should return courseDto list")]
        public async Task GetAll_WhenBoletosExists_ShouldReturnCourseDtoList()
        {
            // Arrange
            var boletos = new List<Boleto>
            {
                new Boleto(
                    "José Augusto",
                    "777.356.450-74",
                    "Pedro Henrique",
                    "93.649.615/0001-24",
                    100.0m,
                    new DateTime(2025, 10, 29),
                    Guid.NewGuid()
                ),
                new Boleto(
                    "Antônio Silva",
                    "44.406.658/0001-73",
                    "Jorge Amado",
                    "432.173.110-00",
                    400.0m,
                    new DateTime(2025, 11, 29),
                    Guid.NewGuid()
                ),
            };

            _repositoryMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(boletos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact(DisplayName = "GetAll when any boleto exists should return empty list")]
        public async Task GetAll_WhenNoBoletosExists_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyListFromRepo = new List<Boleto>();

            _repositoryMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(emptyListFromRepo);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact(DisplayName = "Update when valid input should return correct result")]
        public async Task Update_WhenValidInput_ShouldReturnCorrectResult()
        {
            //Arrange
            var boletoId = Guid.NewGuid();
            var boletoDb = new Boleto(
                "Antônio Silva",
                "44.406.658/0001-73",
                "Jorge Amado",
                "432.173.110-00",
                400.0m,
                new DateTime(2025, 11, 29),
                Guid.NewGuid()
            );
            boletoDb.Id = boletoId;

            var boletoUpdated = new BoletoDto()
            {
                Id = boletoId,
                NomeDoPagador = "José Augusto",
                DocumentoDoPagador = "777.356.450-74",
                NomeDoBeneficiario = "Pedro Henrique",
                DocumentoDoBeneficiario = "93.649.615/0001-24",
                Valor = 100.00m,
                DataDeVencimento = new DateTime(2025, 10, 29),
                BancoId = Guid.NewGuid(),
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(boletoId)).ReturnsAsync(boletoDb);

            //Act
            await _service.UpdateAsync(boletoUpdated);

            // Assert
            _uowMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once()
            );
        }

        [Theory(DisplayName = "Update when data is invalid should throw ArgumentException")]
        [InlineData(
            "",
            "777.356.450-74",
            "Pedro Henrique",
            "93.649.615/0001-24",
            "O nome do pagador é obrigatório"
        )]
        [InlineData(
            "José Augusto",
            "777.356.450-00",
            "Pedro Henrique",
            "93.649.615/0001-24",
            "O documento do pagador é inválido."
        )]
        [InlineData(
            "José Augusto",
            "777.356.450-74",
            "",
            "93.649.615/0001-24",
            "O nome do beneficiário é obrigatório."
        )]
        [InlineData(
            "José Augusto",
            "777.356.450-74",
            "Pedro Henrique",
            "93.649.615/0001-00",
            "O documento do beneficiário é inválido."
        )]
        public async Task Update_WhenDataIsInvalid_ShouldThrowArgumentException(
            string nomePagador,
            string documentoDoPagador,
            string nomeDoBeneficiario,
            string documentoDoBeneficiario,
            string expectedMessage
        )
        {
            //Arrange
            var boletoId = Guid.NewGuid();
            var boletoDb = new Boleto(
                "Antônio Silva",
                "44.406.658/0001-73",
                "Jorge Amado",
                "432.173.110-00",
                400.0m,
                new DateTime(2025, 11, 29),
                Guid.NewGuid()
            );
            boletoDb.Id = boletoId;

            var boletoUpdated = new BoletoDto()
            {
                Id = boletoId,
                NomeDoPagador = nomePagador,
                DocumentoDoPagador = documentoDoPagador,
                NomeDoBeneficiario = nomeDoBeneficiario,
                DocumentoDoBeneficiario = documentoDoBeneficiario,
                Valor = 400.00m,
                DataDeVencimento = new DateTime(2025, 10, 29),
                BancoId = Guid.NewGuid(),
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(boletoId)).ReturnsAsync(boletoDb);

            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.UpdateAsync(boletoUpdated)
            );
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "Delete when boleto exists should return correct result")]
        public async Task Delete_WhenBoletoExists_ShouldReturnCorrectResult()
        {
            //arrange
            var boletoId = Guid.NewGuid();
            var boletoDb = new Boleto(
                "Antônio Silva",
                "44.406.658/0001-73",
                "Jorge Amado",
                "432.173.110-00",
                400.0m,
                new DateTime(2025, 11, 29),
                Guid.NewGuid()
            );
            boletoDb.Id = boletoId;

            _repositoryMock.Setup(r => r.GetByIdAsync(boletoId)).ReturnsAsync(boletoDb);

            //act
            await _service.DeleteAsync(boletoId);

            //assert
            _repositoryMock.Verify(r => r.Delete(boletoDb), Times.Once);
            _uowMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
