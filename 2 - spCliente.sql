CREATE PROC ClienteCrear(@Nombre varchar(45),@Apellido varchar(45), @Dni varchar(9))
AS
BEGIN
	INSERT INTO Cliente (Nombre, Apellido, Dni) VALUES (@Nombre, @Apellido, @Dni)
END
GO

CREATE PROC ClienteActualizar(@IdCliente int, @Nombre varchar(45),@Apellido varchar(45))
AS
BEGIN
	UPDATE Cliente SET Nombre=@Nombre, Apellido=@Apellido WHERE IdCliente = @IdCliente
END
GO

CREATE PROC ClienteEliminar(@IdCliente int)
AS
BEGIN
	DELETE Cliente WHERE IdCliente = @IdCliente
END
GO

CREATE PROC ClienteListar
AS
BEGIN
	SELECT * FROM Cliente
END
GO

CREATE PROC ClienteTraerPorID(@IdCliente int)
AS
BEGIN
	SELECT *
	FROM Cliente
	WHERE IdCliente = @IdCliente
END
GO

CREATE PROC ClienteTraerPorDni(@Dni varchar(9))
AS
BEGIN
	SELECT *
	FROM Cliente
	WHERE Dni = @Dni
END
GO