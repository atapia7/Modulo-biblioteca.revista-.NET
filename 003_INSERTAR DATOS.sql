CREATE PROCEDURE usp_inserta_Editorial
@nombre varchar(50),
@fecha date,
@estado int 
AS BEGIN 
INSERT INTO EDITORIAL VALUES (@nombre,@fecha,@estado)
END

CREATE PROCEDURE usp_actualiza_Editorial
@cod INT,
@nombre VARCHAR(50),
@fecha DATE,
@estado INT AS BEGIN
UPDATE dbo.EDITORIAL SET Nombre=@nombre,fecha=@fecha,IdEstado=@estado WHERE IdEditorial=@cod END

CREATE PROCEDURE usp_inserta_autor 
@Descripcion   VARCHAR(50),
@IdEstado     INT
AS 
BEGIN 
INSERT INTO AUTOR VALUES(@Descripcion,@IdEstado)
END

SELECT A.IdAutor,A.Descripcion,E.IdEstado,E.NombreEstado FROM AUTOR A JOIN ESTADO_AUTOR E ON A.IdEstado=E.IdEstado 

CREATE PROCEDURE usp_actualiza_Autor
@cod int,
@Descripcion varchar(50),
@IdEstado int
AS BEGIN 
UPDATE  AUTOR SET Descripcion=@Descripcion, IdEstado=@IdEstado WHERE idAutor=@cod END

CREATE PROCEDURE usp_inserta_Categoria
@Nombre varchar(50),
@IdEstado int
AS BEGIN
INSERT INTO categoria VALUES(@Nombre,@IdEstado)
END	

CREATE PROCEDURE usp_actualiza_Categoria
@cod int,
@Descripcion varchar(50),
@IdEstado int
AS BEGIN 
UPDATE  CATEGORIA SET Nombre=@Descripcion, IdEstado=@IdEstado WHERE IdCategoria=@cod END


CREATE PROC listalibro 
AS begin
SELECT L.IdLibro,L.Titulo ,
L.NombrePortada ,A.IdAutor ,A.Descripcion,
C.IdCategoria,C.Nombre,E.IdEditorial,E.Nombre,
L.Ubicacion, L.Ejemplares,L.Estado
FROM LIBRO L JOIN AUTOR A ON A.IdAutor=L.IdAutor
join CATEGORIA C ON C.IdCategoria=L.IdCategoria
JOIN EDITORIAL E ON E.IdEditorial=L.IdEditorial
END
EXEC listalibro

CREATE PROC usp_inserta_Libro (
@Titulo varchar(100),
@NombrePortada varchar(100),
@IdAutor int,
@IdCategoria int,
@IdEditorial int,
@Ubicacion varchar(100),
@Ejemplares int,
@Estado varchar(20))
as
begin
insert into LIBRO(Titulo,NombrePortada,IdAutor,IdCategoria,IdEditorial,Ubicacion,Ejemplares,Estado) values (
@Titulo,@NombrePortada,@IdAutor,@IdCategoria,@IdEditorial,@Ubicacion,@Ejemplares,@Estado)
end

CREATE PROC usp_actualiza_libro(
@cod int ,
@Titulo varchar(100),
@NombrePortada varchar(100),
@IdAutor int,
@IdCategoria int,
@IdEditorial int,
@Ubicacion varchar(100),
@Ejemplares int,
@Estado varchar(20))
AS begin
update LIBRO set 
		Titulo = @Titulo,		IdAutor = @IdAutor,
		IdCategoria = @IdCategoria,		IdEditorial = @IdEditorial,
		Ubicacion = @Ubicacion,		Ejemplares = @Ejemplares,
		Estado = @Estado
		where IdLibro = @cod
end

