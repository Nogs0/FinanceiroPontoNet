using FinanceiroPontoNet.Application.Bancos;
using FinanceiroPontoNet.Application.Bancos.Dtos;
using FinanceiroPontoNet.Domain.Bancos;
using FinanceiroPontoNet.Domain.Shared.Exceptions;
using FinanceiroPontoNet.Domain.Shared.UnitOfWork;
using Moq;

namespace FinanceiroPontoNet.Tests.Application.Bancos
{
    public class BancoServiceTest
    {
        private readonly IBancoService _service;
        private readonly Mock<IBancoRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _uowMock;

        public BancoServiceTest()
        {
            _repositoryMock = new Mock<IBancoRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _service = new BancoService(_repositoryMock.Object, _uowMock.Object);
        }

        [Fact(DisplayName = "Create when valid input should return correct result")]
        public async Task Create_WhenValidInput_ShouldReturnCorrectResult()
        {
            //Arrange
            var createDto = new CreateBancoDto()
            {
                Nome = "Banco 1",
                Codigo = "1",
                PercentualDeJuros = 0.1m,
            };

            //Act
            var result = await _service.CreateAsync(createDto);

            //Assert
            _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Banco>()), Times.Once());

            _uowMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once()
            );
        }

        [Fact(DisplayName = "Create when nome is empty should throw ArgumentException")]
        public async Task Create_WhenNomeIsEmpty_ShouldThrowArgumentException()
        {
            //Arrange
            var createDto = new CreateBancoDto()
            {
                Nome = "",
                Codigo = "",
                PercentualDeJuros = 0.1m,
            };

            var expectedMessage = "O nome do banco é obrigatório";

            //Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Banco(createDto.Nome, createDto.Codigo, createDto.PercentualDeJuros)
            );

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "Create when código is empty should throw ArgumentException")]
        public async Task Create_WhenCodigoIsEmpty_ShouldThrowArgumentException()
        {
            //Arrange
            var createDto = new CreateBancoDto()
            {
                Nome = "Banco 1",
                Codigo = "",
                PercentualDeJuros = 0.1m,
            };

            var expectedMessage = "O código do banco é obrigatório";

            //Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Banco(createDto.Nome, createDto.Codigo, createDto.PercentualDeJuros)
            );

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "Get when id exists should return correct result")]
        public async Task Get_WhenBancoIdExists_ShouldReturnCorrectResult()
        {
            //Arrange
            var bancoId = Guid.NewGuid();
            var bancoDb = new Banco("Banco existente", "1", 0.1m);
            bancoDb.Id = bancoId;

            _repositoryMock.Setup(r => r.GetByIdAsync(bancoId)).ReturnsAsync(bancoDb);

            //Act
            var result = await _service.GetAsync(bancoId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BancoDto>(result);
            Assert.Equal(result.Id, bancoId);
        }

        [Fact(DisplayName = "Get when código exists should return correct result")]
        public async Task Get_WhenBancoCodigoExists_ShouldReturnCorrectResult()
        {
            //Arrange
            var bancoCodigo = "1";
            var bancoDb = new Banco("Banco existente", bancoCodigo, 0.1m);
            bancoDb.Id = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetByCodigoAsync(bancoCodigo)).ReturnsAsync(bancoDb);

            //Act
            var result = await _service.GetByCodigoAsync(bancoCodigo);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BancoDto>(result);
            Assert.Equal(result.Codigo, bancoCodigo);
        }

        [Fact(DisplayName = "Get when id is not found")]
        public async Task Get_WhenBancoIdIsNotFound_ShouldThrowNotFoundException()
        {
            //Arrange
            var bancoId = Guid.NewGuid();

            var expectedMessage =
                $"A entidade 'Banco' com a chave: '{bancoId}' não foi encontrada.";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.GetAsync(bancoId)
            );

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "Get when codigo is not found")]
        public async Task Get_WhenBancoCodigoIsNotFound_ShouldThrowNotFoundException()
        {
            //Arrange
            var bancoCodigo = "1";

            var expectedMessage =
                $"A entidade 'Banco' com a chave: '{bancoCodigo}' não foi encontrada.";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.GetByCodigoAsync(bancoCodigo)
            );

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "GetAll when bancos exists should return courseDto list")]
        public async Task GetAll_WhenBancosExists_ShouldReturnCourseDtoList()
        {
            // Arrange
            var bancos = new List<Banco>
            {
                new Banco("Algoritmo e Estrutura de Dados I", "1", 0.1m),
                new Banco("Cálculo I", "2", 0.2m),
            };

            _repositoryMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(bancos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact(DisplayName = "GetAll when any banco exists should return empty list")]
        public async Task GetAll_WhenNoBancosExists_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyListFromRepo = new List<Banco>();

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
            var bancoId = Guid.NewGuid();
            var bancoDb = new Banco("Banco existente", "1", 0.1m);

            var bancoUpdated = new BancoDto()
            {
                Id = bancoId,
                Nome = "Banco atualizado",
                Codigo = "1",
                PercentualDeJuros = 0.2m,
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(bancoId)).ReturnsAsync(bancoDb);

            //Act
            await _service.UpdateAsync(bancoUpdated);

            // Assert
            _uowMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once()
            );
        }

        [Fact(DisplayName = "Update when nome is empty should throw ArgumentException")]
        public async Task Update_WhenNomeIsEmpty_ShouldThrowArgumentException()
        {
            //Arrange
            var bancoId = Guid.NewGuid();
            var bancoDb = new Banco("Banco existente", "1", 0.1m);

            var bancoUpdated = new BancoDto()
            {
                Id = bancoId,
                Nome = "",
                Codigo = "1",
                PercentualDeJuros = 0.2m,
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(bancoId)).ReturnsAsync(bancoDb);
            var expectedMessage = "O nome do banco é obrigatório";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.UpdateAsync(bancoUpdated)
            );
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "Update when código is empty should throw ArgumentException")]
        public async Task Update_WhenNameIsEmpty_ShouldThrowArgumentException()
        {
            //Arrange
            var bancoId = Guid.NewGuid();
            var bancoDb = new Banco("Banco existente", "1", 0.1m);

            var bancoUpdated = new BancoDto()
            {
                Id = bancoId,
                Nome = "Banco atualizado",
                Codigo = "",
                PercentualDeJuros = 0.2m,
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(bancoId)).ReturnsAsync(bancoDb);
            var expectedMessage = "O código do banco é obrigatório";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.UpdateAsync(bancoUpdated)
            );
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "Delete when banco exists should return correct result")]
        public async Task Delete_WhenBancoExists_ShouldReturnCorrectResult()
        {
            //arrange
            var bancoId = Guid.NewGuid();
            var bancoDb = new Banco("Banco existente", "1", 01m);
            bancoDb.Id = bancoId;

            _repositoryMock.Setup(r => r.GetByIdAsync(bancoId)).ReturnsAsync(bancoDb);

            //act
            await _service.DeleteAsync(bancoId);

            //assert
            _repositoryMock.Verify(r => r.Delete(bancoDb), Times.Once);
            _uowMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
