create database helloworld;
use helloworld;
CREATE TABLE Usuarios (
    id_usuario INT PRIMARY KEY AUTO_INCREMENT,
    usuario VARCHAR(15) unique,
    nombre varchar(100),
    correo VARCHAR(100) UNIQUE,
    telefono VARCHAR(20),
    direccion VARCHAR(200),
    tipo_usuario ENUM('Turista', 'Guía') DEFAULT 'Turista',
    contrasena VARCHAR(255), 
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    foto_perfil VARCHAR(255),
    idiomas VARCHAR(255),
    nacionalidad varchar(40),
    especialidad VARCHAR(255),
    experiencia TEXT,
    disponilidad text, 
    tarifaHora decimal (10,2),
    tarifaTour decimal(10,2)
    

);






CREATE TABLE Reservas (
    id_reserva INT PRIMARY KEY AUTO_INCREMENT,
    id_turista INT,           
    id_guia INT,               
    fecha_reserva TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  
    fecha_programada DATE,     
    descripcion TEXT,
    duracion TIME,
    precio DECIMAL(10, 2),
    estado ENUM('Pendiente', 'Confirmada', 'Cancelada', 'Completada') DEFAULT 'Pendiente',
    FOREIGN KEY (id_turista) REFERENCES Usuarios(id_usuario),
    FOREIGN KEY (id_guia) REFERENCES Usuarios(id_usuario)
);

drop database helloworld;


CREATE TABLE Planes (
    id_plan INT PRIMARY KEY AUTO_INCREMENT,
    id_guia INT,               
    nombre_plan VARCHAR(255),  
    descripcion TEXT,          
    duracion varchar(100),               
    numero_personas varchar(100),       
    precio DECIMAL(10, 2),     
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP, 
    FOREIGN KEY (id_guia) REFERENCES Usuarios(id_usuario)
);


ALTER TABLE Mensaje
ADD COLUMN ImagenRuta VARCHAR(255) NULL;
