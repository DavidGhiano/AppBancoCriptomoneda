
CREATE PROC HistorialCrear(
							@Fecha datetime,
							@IdCuenta int,
							@Monto decimal(15,6),
							@Tipo varchar(20)
)
AS
BEGIN
	INSERT INTO Historial (Fecha, IdCuenta, Monto, Tipo) VALUES(@Fecha,@IdCuenta,@Monto,@Tipo)
END
GO

CREATE PROC HistorialListar(@IdCuenta int)
AS
BEGIN
	SELECT IdHistorial, Fecha, IdCuenta, Monto, Tipo
	FROM Historial
	WHERE IdCuenta = @IdCuenta
	ORDER BY Fecha DESC
END