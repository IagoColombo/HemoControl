CREATE DATABASE IF NOT EXISTS hemocontrol;
USE hemocontrol;

CREATE TABLE pacientes (
    id                    INT AUTO_INCREMENT PRIMARY KEY,
    nome                  VARCHAR(150)  NOT NULL,
    cpf                   VARCHAR(14)   NOT NULL,
    tipo_hemofilia        VARCHAR(50)   NOT NULL,
    dose_prescrita        DECIMAL(10,2) NOT NULL,
    aplicacoes_por_semana INT           NOT NULL
);

CREATE TABLE retiradas (
    id           INT AUTO_INCREMENT PRIMARY KEY,
    data_retirada DATE          NOT NULL,
    quantidade_ui DECIMAL(12,2) NOT NULL,
    hemocentro    VARCHAR(200)  NOT NULL,
    pacienteid    INT           NOT NULL,
    FOREIGN KEY (pacienteid) REFERENCES pacientes(id)
);
