CREATE TABLE Opiniones (
    IdOpinion INT AUTO_INCREMENT PRIMARY KEY,
    IdTurista INT NOT NULL,
    IdGuia INT NOT NULL,
    Calificacion INT NOT NULL CHECK (Calificacion BETWEEN 1 AND 5),
    Comentario VARCHAR(1000) NOT NULL,
    Fecha DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_Opiniones_Turista FOREIGN KEY (IdTurista)
        REFERENCES Usuarios(id_usuario)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,

    CONSTRAINT FK_Opiniones_Guia FOREIGN KEY (IdGuia)
        REFERENCES Usuarios(id_usuario)
        ON DELETE RESTRICT
        ON UPDATE CASCADE
);
