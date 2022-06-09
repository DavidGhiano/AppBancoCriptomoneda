CREATE PROC CuentaCrear(@Tipo varchar(10), @IdCliente int)
AS
BEGIN
	INSERT INTO Cuenta(Saldo, Tipo, IdCliente) output INSERTED.IdCuenta VALUES (0.0, @Tipo, @IdCliente)
END
GO

CREATE PROC CuentaActualizar(@Saldo decimal(15,6), @IdCuenta int)
AS
BEGIN
	UPDATE Cuenta SET Saldo=@Saldo WHERE IdCuenta = @IdCuenta
END
GO

CREATE PROC CuentaEliminar(@IdCuenta int)
AS
BEGIN
	DELETE Cuenta WHERE IdCuenta = @IdCuenta
END
GO

CREATE PROC ObtenerCuenta(@IdCuenta int)
AS
BEGIN
	SELECT Tipo
	FROM Cuenta
	WHERE IdCuenta = @IdCuenta
END
GO



--FIDUCIARIAS

CREATE PROC CuentaFiduciariaObtener(@IdCuenta int)
AS
BEGIN
	SELECT C.IdCuenta, C.Saldo, C.Tipo, C.IdCliente, F.Cbu, F.Alias, F.NroCuenta
	FROM Cuenta C INNER JOIN Fiduciaria F
	ON C.IdCuenta = F.IdCuenta
	WHERE C.IdCuenta = @IdCuenta
END
GO

CREATE PROC CuentaFiduciariaListar
AS
BEGIN
	SELECT C.IdCuenta, C.Saldo, C.Tipo, C.IdCliente, F.Cbu, F.Alias, F.NroCuenta
	FROM Cuenta C INNER JOIN Fiduciaria F
	ON C.IdCuenta = F.IdCuenta
	WHERE 1=1
	OR C.Tipo = 'ARG' 
	OR C.Tipo = 'USD'
END
GO

CREATE PROC CuentaFiduciariaCrear(
									@IdCuenta int,
									@Cbu varchar(22),
									@Alias varchar(45),
									@NroCuenta int
)
AS
BEGIN
	INSERT INTO Fiduciaria(IdCuenta,Cbu,Alias,NroCuenta) VALUES (@IdCuenta, @Cbu, @Alias, @NroCuenta)
END

-- CRIPTO
CREATE PROC CuentaCriptoCrear(
								@IdCuenta int,
								@Uuid varchar(22)
)
AS
BEGIN
	INSERT INTO Cripto (IdCuenta,Uuid) VALUES (@IdCuenta,@Uuid)
END

CREATE PROC CuentaCriptoListar
AS
BEGIN
	SELECT Cu.IdCuenta, Cu.Saldo, Cu.Tipo, Cu.IdCliente, Cr.Uuid
	FROM Cuenta Cu INNER JOIN Cripto Cr
	ON Cu.IdCuenta = Cr.IdCuenta
	WHERE Cu.Tipo = 'BTC'
END
GO

CREATE PROC CuentaCriptoObtener(@IdCuenta int)
AS
BEGIN
	SELECT Cu.IdCuenta, Cu.Saldo, Cu.Tipo, Cu.IdCliente, Cr.Uuid
	FROM Cuenta Cu INNER JOIN Cripto Cr
	ON Cu.IdCuenta = Cr.IdCuenta
	WHERE Cu.IdCuenta = @IdCuenta
END
GO

