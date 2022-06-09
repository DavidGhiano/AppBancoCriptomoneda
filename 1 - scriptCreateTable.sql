CREATE TABLE Cliente(
		IdCliente int PRIMARY KEY IDENTITY(1,1),
		Nombre varchar(45),
		Apellido varchar(45),
		Dni varchar(9)
)
GO

CREATE TABLE Cuenta(
		IdCuenta int PRIMARY KEY IDENTITY(1,1),
		Saldo decimal(15,6) NOT NULL,
		Tipo varchar(10)
)
GO

CREATE TABLE Fiduciaria(
		IdCuenta int,
		Cbu varchar(22),
		Alias varchar(45),
		NroCuenta int
)
GO

CREATE TABLE Cripto(
		IdCuenta int,
		Uuid varchar(22)
)
GO

CREATE TABLE Historial(
		IdHistorial int PRIMARY KEY IDENTITY(1,1),
		Fecha datetime,
		IdCuenta int,
		Saldo decimal(15,6),
		Tipo varchar(20)
)
GO

INSERT INTO Cliente (Nombre,Apellido,Dni) VALUES ('Pedro','Picapiedra','12456987')
GO
