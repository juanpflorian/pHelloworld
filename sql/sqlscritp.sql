use helloworld;
select * from __efmigrationshistory;
select * from reservas;
select * from usuarios ;
select * from pLanes;
SELECT U.id_usuario
FROM Planes P 


INNER JOIN Usuarios U ON P.id_guia = U.id_usuario
WHERE P.id_plan = 3; -- Reemplaza "1" con el ID de un plan válido
SELECT * FROM Usuarios WHERE correo = 'verde@gmail.com';

SELECT p.id_plan, u.nombre AS nombre_guia, p.nombre_plan, p.descripcion, p.duracion, p.numero_personas, p.precio
FROM Planes p
JOIN Usuarios u ON p.id_guia = u.id_usuario;

ALTER TABLE Usuarios 
MODIFY COLUMN Contrasena VARCHAR(255) NOT NULL;


ALTER TABLE Usuarios MODIFY COLUMN correo VARCHAR(100) NOT NULL;


select foto_perfil from usuarios where foto_perfil = null;

desc reservas;


alter table reservas MODIFY COLUMN duracion Varchar(100);

