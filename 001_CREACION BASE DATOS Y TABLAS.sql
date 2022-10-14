CREATE DATABASE DB_BIBLIOTECA
GO

USE DB_BIBLIOTECA

create table tb_users(
id int primary key references PERSONA(IdPersona),
logins varchar(50) unique,
clave varchar(10),
intentos int check(intentos<=3) default(0),
fecBloque datetime null) 

insert into tb_users values (1,'cibertec','1234',0,null),(2,'admin','1234',0,null)

create or alter proc usp_verifica_acceso
@login varchar(20),
@clave varchar(10),
@fullname varchar(150) output,
@sw varchar(1) output
As
BEGIN
	declare @id int = (Select top 1 id from tb_users Where logins=@login AND clave=@clave)
	if(@id is null)
	begin
	set @sw='0'
		Set @fullname='Usuario o Clave Incorrecta'
		end
	else
	begin
	    set	@sw='1'
		Set @fullname=(SELECT CONCAT(p.Nombre,' ',p.Apellido) FROM PERSONA p inner join tb_users u on u.id=p.IdPersona
		WHERE IdPersona=@id)
end
end

select * from tb_users
select * from PERSONA

CREATE TABLE ESTADO_AUTOR(
IdEstado int primary key identity(100,1),
NombreEstado varchar(30))

CREATE TABLE  AUTOR(
IdAutor       INT PRIMARY KEY IDENTITY(100,1),
Descripcion   VARCHAR(50),
IdEstado     INT REFERENCES ESTADO_AUTOR(IdEstado))

CREATE TABLE ESTADO_CATEGORIA(
IdEstado INT PRIMARY KEY IDENTITY(100,11),
NombreEstado VARCHAR(30))

CREATE TABLE  CATEGORIA(
IdCategoria INT PRIMARY KEY IDENTITY(100,1),
Nombre     VARCHAR(50),
IdEstado INT REFERENCES ESTADO_CATEGORIA(IdEstado))  

SELECT a.IdCategoria,a.Nombre,e.IdEstado,e.NombreEstado FROM CATEGORIA a JOIN ESTADO_CATEGORIA e ON a.IdEstado=e.IdEstado 


CREATE TABLE ESTADO_EDITORIAL(
IdEstado INT PRIMARY KEY IDENTITY(100,1),
NombreEstado VARCHAR(30))

CREATE TABLE  EDITORIAL(
IdEditorial    INT PRIMARY KEY IDENTITY,
Nombre    VARCHAR(50),
fecha DATE,
IdEstado INT REFERENCES ESTADO_EDITORIAL(IdEstado))
SELECT a.IdEditorial,a.Nombre,a.fecha,a.IdEstado,e.NombreEstado FROM EDITORIAL a JOIN ESTADO_EDITORIAL e ON a.IdEstado=e.IdEstado 
go


CREATE TABLE LIBRO(
IdLibro INT PRIMARY KEY IDENTITY(100,1),
Titulo VARCHAR(100),
NombrePortada VARCHAR(100),
IdAutor INT REFERENCES AUTOR(IdAutor),
IdCategoria INT REFERENCES CATEGORIA(IdCategoria),
IdEditorial INT REFERENCES EDITORIAL(IdEditorial),
Ubicacion VARCHAR(50),
Ejemplares INT DEFAULT 1,
Estado VARCHAR(40))
GO

CREATE TABLE TIPO_PERSONA(
IdTipoPersona  int primary key,
Descripcion varchar(50),
Estado bit default 1,
FechaCreacion datetime default getdate()
)
GO

CREATE TABLE PERSONA(
IdPersona int primary key identity,
Nombre varchar(50),
Apellido varchar(50),
Correo varchar(50),
Clave varchar(50),
Codigo varchar(50),
IdTipoPersona int references TIPO_PERSONA(IdTipoPersona),
Estado bit default 1,
FechaCreacion datetime default getdate()
)
go

CREATE TABLE ESTADO_PRESTAMO(
IdEstadoPrestamo int primary key,
Descripcion varchar(50),
Estado bit default 1,
FechaCreacion datetime default getdate()
)
GO

CREATE TABLE PRESTAMO(
IdPrestamo int primary key identity,
IdEstadoPrestamo int references ESTADO_PRESTAMO(IdEstadoPrestamo),
IdPersona int references PERSONA(IdPersona),
IdLibro int references Libro(IdLibro),
FechaDevolucion datetime,
FechaConfirmacionDevolucion datetime,
EstadoEntregado varchar(100),
EstadoRecibido varchar(100),
Estado bit default 1,
FechaCreacion datetime default getdate()
)
go

INSERT INTO ESTADO_AUTOR VALUES('AUTOR CONTEMPORANEO')
INSERT INTO ESTADO_AUTOR VALUES('AUTOR MODERNO')
INSERT INTO ESTADO_AUTOR VALUES('AUTOR OLD')
INSERT INTO ESTADO_EDITORIAL VALUES('ACTIVO EDITORIAL 1')
INSERT INTO ESTADO_EDITORIAL VALUES('ACTIVO EDITORIAL 2')
INSERT INTO ESTADO_EDITORIAL VALUES('ACTIVO EDITORIAL 3')
INSERT INTO ESTADO_CATEGORIA VALUES('DESACTIVADO')
INSERT INTO ESTADO_CATEGORIA VALUES('ACTIVO')

--------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------
------------[Procedures]--------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE usp_inserta_Editorial
@nombre varchar(50),
@fecha date,
@estado int 
AS 
BEGIN 
INSERT INTO EDITORIAL VALUES (@nombre,@fecha,@estado)
END


CREATE PROCEDURE usp_actualiza_Editorial
@cod INT,
@nombre VARCHAR(50),
@fecha DATE,
@estado INT AS BEGIN
UPDATE dbo.EDITORIAL SET Nombre=@nombre,fecha=@fecha,IdEstado=@estado WHERE IdEditorial=@cod 
END
go

CREATE PROCEDURE usp_inserta_autor 
@Descripcion   VARCHAR(50),
@IdEstado     INT
AS 
BEGIN 
INSERT INTO AUTOR VALUES(@Descripcion,@IdEstado)
END

SELECT A.IdAutor,A.Descripcion,E.IdEstado,E.NombreEstado FROM AUTOR A JOIN ESTADO_AUTOR E ON A.IdEstado=E.IdEstado 
go

CREATE PROCEDURE usp_actualiza_Autor
@cod int,
@Descripcion varchar(50),
@IdEstado int
AS BEGIN 
UPDATE  AUTOR SET Descripcion=@Descripcion, IdEstado=@IdEstado WHERE idAutor=@cod END
go

CREATE PROCEDURE usp_inserta_Categoria
@Nombre varchar(50),
@IdEstado int
AS BEGIN
INSERT INTO categoria VALUES(@Nombre,@IdEstado)
END
go

CREATE PROCEDURE usp_actualiza_Categoria
@cod int,
@Descripcion varchar(50),
@IdEstado int
AS BEGIN 
UPDATE  CATEGORIA SET Nombre=@Descripcion, IdEstado=@IdEstado WHERE IdCategoria=@cod END
go

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
go

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
go


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
go

select*from AUTOR
go




-----------------------------------------------------------------
insert into TIPO_PERSONA(IdTipoPersona, Descripcion) values
(1,'Administrador'),
(2,'Empleado'),
(3,'Lector')
GO
insert into PERSONA(nombre,apellido,correo,clave,IdTipoPersona) values
('Andree','Tapia','a@gmail.com','123',1),
('Jamber','Torres','a@gmail.com','456',2)



go
select * from PERSONA
GO

INSERT INTO ESTADO_PRESTAMO(IdEstadoPrestamo,Descripcion) VALUES
(1,'Pendiente'),
(2,'Devuelto')

create  procedure usp_insertaPrestamo
@IdPersona int,
@IdLibro int,
@FechaDevolucion datetime,
@EstadoEntregado varchar(100)
as
begin
	insert into PRESTAMO values(1,@IdPersona,@IdLibro,@FechaDevolucion,GETDATE(),@EstadoEntregado,'Aun no recibido','1',GETDATE())
end
go

	GO


CREATE TABLE REVISTAS (
  IdRevista int primary key,
  NombreRevista varchar(40) not null,
  umedida varchar(100),
  PrecioUnidad decimal(10,0) not null,
  UnidadesEnExistencia smallint not null)
go
insert into REVISTAS values('1','Ambito','Revistas','12.00','10')
insert into REVISTAS values('2','Areas','Revistas','13.00','40')
insert into REVISTAS values('3','Arquitextos','Revistas','14.00','20')
insert into REVISTAS values('4','Aula','Revistas','12.00','30')
insert into REVISTAS values('5','Bazaar','Revistas','16.50','70')
insert into REVISTAS values('6','Bicentenario','Revistas','13.20','90')
insert into REVISTAS values('7','Ciencia Covid','Revistas','12.00','50')
insert into REVISTAS values('8','Cuadernos','Revistas','12.50','20')
insert into REVISTAS values('9','Educacion, Arte Y Comunicacion','Revistas','12.30','80')
insert into REVISTAS values('10','Hallazgos21','Revistas','20.00','20')
insert into REVISTAS values('11','Herencia','Revistas','13.00','70')
insert into REVISTAS values('12','Ingenieria Biometrica','Revistas','60.00','10')
insert into REVISTAS values('13','IngeTecno','Revistas','10.00','30')
insert into REVISTAS values('14','Revista De Historia','Revistas','13.00','50')
insert into REVISTAS values('15','Revista Economica','Revistas','16.00','30')
insert into REVISTAS values('16','RollingStone','Revistas','19.00','10')
insert into REVISTAS values('17','Time','Revistas','15.00','50')
insert into REVISTAS values('18','Trama','Revistas','17.00','20')
go
select*from REVISTAS
GO

create  proc usp_Revistas
as
begin
		select r.IdRevista, r.NombreRevista, r.PrecioUnidad, r.UnidadesEnExistencia as stock
		from REVISTAS r
end
go


create table PEDIDO(
	idpedido varchar(8) primary key,
	fpedido datetime default getdate(),
	dnicli varchar(8),
	nombcli varchar(100),
	monto decimal(10,2)
)
go
create table DETAPEDIDO(
	idpedido varchar(8) references PEDIDO,
	IdRevista int references REVISTAS,
	precio decimal,
	cantidad int
)
go
create  function dbo.autogenerado() returns varchar(8)
as
begin
	declare @n int
	declare @idpedido varchar(8)=(Select top 1 idpedido FROM PEDIDO order by 1 desc)
	if(@idpedido is null)
		set @n=1
	else
		set @n=CAST(@idpedido as int)+1
	return Concat(replicate ('0',8-Len(@n)),@n)

end
go

print dbo.autogenerado()
go


create  procedure usp_pedido_agrega
@idpedido varchar(8) output,
@dni varchar(8),
@nombre varchar(100),
@monto decimal(10,2)
as
begin
	set @idpedido=dbo.autogenerado()
	insert into PEDIDO(idpedido,dnicli,nombcli,monto)
	values(@idpedido, @dni,@nombre,@monto)
end
go

CREATE PROCEDURE usp_detapedido_agregar
@idpedido varchar(8),
@idrevista int,
@precio decimal,
@cantidad int
as
begin
	insert DETAPEDIDO values(@idpedido,@idrevista,@precio,@cantidad)
end
go


create procedure usp_revistas_ActualizarStock
@codigo int,
@cantidad int
as
begin
	
	UPDATE REVISTAS set UnidadesEnExistencia-=@cantidad
	where IdRevista=@codigo
end
go

create proc usp_listaPrestamo
as begin
select p.IdPrestamo,p.IdEstadoPrestamo,ep.Descripcion ,p.IdPersona,p2.Nombre,p.IdLibro,l.Titulo, p.FechaDevolucion,p.FechaConfirmacionDevolucion,p.EstadoEntregado,
p.EstadoRecibido,p.Estado,p.FechaCreacion from PRESTAMO p inner join ESTADO_PRESTAMO ep on ep.IdEstadoPrestamo=p.IdEstadoPrestamo 
inner join PERSONA p2 on p2.IdPersona=p.IdPersona inner join LIBRO l on l.IdLibro=p.IdLibro 
end


create or alter procedure usp_InsertaPersona_adm
@Nombres varchar(50),
@Apellidos varchar(50),
@correo varchar(50),
@clave varchar(50),
@idTipoPersona int
as
begin
	insert into PERSONA(nombre,apellido,correo,clave,IdTipoPersona) values(@Nombres,@Apellidos,@correo,@clave,@idTipoPersona)
end
go

execute usp_InsertaPersona_adm 'Carlos','Pichilingue','321@gmail.com','123','1'
go

create or alter procedure usp_actualiza_persona
@codigo int,
@Nombres varchar(50),
@Apellidos varchar(50),
@correo varchar(50),
@clave varchar(50),
@idTipoPersona int
as
begin
update PERSONA set Nombre=@Nombres, Apellido=@Apellidos, Correo=@correo,Clave=@clave, IdTipoPersona=@idTipoPersona
where IdPersona=@codigo
end
go
execute usp_actualiza_persona '2','Julio','Peña','qwe@gmail.com','321','1'
go

select*from PERSONA
go

create or alter procedure usp_listar_Personas
as
begin
		select p.IdPersona, p.Nombre,p.Apellido,p.Correo,p.Clave,t.Descripcion,p.Estado,p.FechaCreacion
		from PERSONA p inner join TIPO_PERSONA t on p.IdTipoPersona=t.IdTipoPersona
end
go

execute usp_listar_Personas

go
