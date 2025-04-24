
USE master;
GO

IF DB_ID('DB_PROJECT_ARQ') IS NOT NULL
BEGIN
    ALTER DATABASE DB_PROJECT_ARQ SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DB_PROJECT_ARQ;
END
GO

CREATE DATABASE DB_PROJECT_ARQ;
GO

USE DB_PROJECT_ARQ;
GO

CREATE TABLE t_usuario (
    usu_id INT PRIMARY KEY IDENTITY(1,1),
    usu_google_id VARCHAR(50) UNIQUE NOT NULL,
    usu_email VARCHAR(100) UNIQUE NOT NULL,
    usu_nombre VARCHAR(50) NOT NULL,
    usu_rol VARCHAR(20) NOT NULL,
    usu_contrasenia VARCHAR(50) NOT NULL,
    usu_codigo_estudiante VARCHAR(20) UNIQUE,
    usu_fecha_registro DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE t_sede (
    sed_id INT PRIMARY KEY IDENTITY(1,1),
    sed_nombre VARCHAR(100) NOT NULL,
    sed_direccion VARCHAR(200),
    sed_latitud VARCHAR(20),
    sed_longitud VARCHAR(20),
    sed_imagen_url VARCHAR(300) NULL
);
GO

CREATE TABLE t_edificio (
    edi_id INT PRIMARY KEY IDENTITY(1,1),
    edi_nombre VARCHAR(50) NOT NULL,
    edi_sed_id INT NOT NULL,
    edi_imagen_url VARCHAR(300) NULL,
    FOREIGN KEY (edi_sed_id) REFERENCES t_sede(sed_id)
);
GO

CREATE TABLE t_bloque (
    blo_id INT PRIMARY KEY IDENTITY(1,1),
    blo_nombre VARCHAR(50) NOT NULL,
    blo_edi_id INT NOT NULL,
    blo_imagen_url VARCHAR(300) NULL,
    FOREIGN KEY (blo_edi_id) REFERENCES t_edificio(edi_id)
);
GO

CREATE TABLE t_salon (
    sal_id INT PRIMARY KEY IDENTITY(1,1),
    sal_codigo INT NOT NULL,
    sal_nombre VARCHAR(20) NOT NULL,
    sal_blo_id INT NOT NULL,
    sal_imagen_url VARCHAR(300) NULL,
    FOREIGN KEY (sal_blo_id) REFERENCES t_bloque(blo_id)
);
GO

CREATE TABLE t_asignatura (
    asi_id INT PRIMARY KEY IDENTITY(1,1),
    asi_nombre VARCHAR(100) NOT NULL,
    asi_docente_id INT NOT NULL,
    asi_docente_nombre VARCHAR(100) NOT NULL
);
GO

CREATE TABLE t_planificacion_asignatura (
    plan_id INT PRIMARY KEY IDENTITY(1,1),
    plan_vigencia DATE NOT NULL,
    plan_estado VARCHAR(20) NOT NULL,
    plan_codigo_estudiante VARCHAR(20) NOT NULL,
    plan_asi_id INT NOT NULL,
    FOREIGN KEY (plan_codigo_estudiante) REFERENCES t_usuario(usu_codigo_estudiante),
    FOREIGN KEY (plan_asi_id) REFERENCES t_asignatura(asi_id)
);
GO

CREATE TABLE t_horario (
    hor_id INT PRIMARY KEY IDENTITY(1,1),
    hor_dia VARCHAR(20) NOT NULL,
    hor_hora_inicio TIME NOT NULL,
    hor_hora_fin TIME NOT NULL,
    hor_fecha_inicio DATE NOT NULL,
    hor_fecha_fin DATE NOT NULL,
    hor_sesiones INT NOT NULL,
    hor_sal_id INT NOT NULL,
    hor_asi_id INT NOT NULL,
    FOREIGN KEY (hor_sal_id) REFERENCES t_salon(sal_id),
    FOREIGN KEY (hor_asi_id) REFERENCES t_asignatura(asi_id)
);
GO

CREATE TABLE t_cambio_aula (
    cam_id INT PRIMARY KEY IDENTITY(1,1),
    cam_fecha_inicio DATE NOT NULL,
    cam_fecha_fin DATE NOT NULL,
    cam_motivo VARCHAR(100) NULL,
	cam_nuevo_sal_id INT NULL,
	cam_hor_id INT NULL,
	FOREIGN KEY (cam_nuevo_sal_id) REFERENCES t_salon(sal_id),
	FOREIGN KEY (cam_hor_id) REFERENCES t_horario(hor_id)
);
GO

CREATE TABLE t_ruta_edificio (
    rut_id INT PRIMARY KEY IDENTITY(1,1),
    rut_edi_id INT NOT NULL, -- Relacionado al edificio
    rut_orden INT NOT NULL,  -- Para ordenar los puntos de la ruta
    rut_latitud VARCHAR(20) NOT NULL,
    rut_longitud VARCHAR(20) NOT NULL,
    FOREIGN KEY (rut_edi_id) REFERENCES t_edificio(edi_id)
);
GO

CREATE TABLE t_notificacion (
    not_id INT PRIMARY KEY IDENTITY(1,1),
    not_usu_id INT NOT NULL,
    not_fecha DATETIME NOT NULL DEFAULT GETDATE(),
    not_mensaje NVARCHAR(300) NOT NULL,
    not_leida BIT NOT NULL DEFAULT 0, -- 0 = no leída, 1 = leída
    not_tipo NVARCHAR(50) DEFAULT 'cambio_aula',
    FOREIGN KEY (not_usu_id) REFERENCES t_usuario(usu_id)
);
GO


CREATE OR ALTER PROCEDURE sp_InsertUsuario
    @usu_google_id NVARCHAR(100) = NULL,
    @usu_email NVARCHAR(100),
    @usu_contrasenia NVARCHAR(100),
    @usu_nombre NVARCHAR(50),
    @usu_id INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @rolDetectado NVARCHAR(20) = 'usuario';
    DECLARE @codigo_estudiante VARCHAR(20) = NULL;

    -- Si es correo institucional, intenta detectar si es estudiante
    IF @usu_email LIKE '%@ucaldas.edu.co'
    BEGIN
        DECLARE @correo_local NVARCHAR(100) = LEFT(@usu_email, CHARINDEX('@', @usu_email) - 1);

        IF @correo_local LIKE '%[0-9]%'
        BEGIN
            DECLARE @i INT = LEN(@correo_local);
            WHILE @i > 0 AND SUBSTRING(@correo_local, @i, 1) LIKE '[0-9]'
            BEGIN
                SET @i = @i - 1;
            END

            SET @codigo_estudiante = SUBSTRING(@correo_local, @i + 1, LEN(@correo_local) - @i);
            SET @rolDetectado = 'estudiante';

            -- Validar si ya existe un usuario con ese código
            IF EXISTS (SELECT 1 FROM t_usuario WHERE usu_codigo_estudiante = @codigo_estudiante)
            BEGIN
                RAISERROR('Ya existe un usuario con este código de estudiante.', 16, 1);
                RETURN;
            END
        END
    END

    -- Insertar el nuevo usuario
    INSERT INTO t_usuario (
        usu_google_id,
        usu_email,
        usu_contrasenia,
        usu_rol,
        usu_nombre,
        usu_codigo_estudiante,
        usu_fecha_registro
    )
    VALUES (
        @usu_google_id,
        @usu_email,
        @usu_contrasenia,
        @rolDetectado,
        @usu_nombre,
        @codigo_estudiante,
        GETDATE()
    );

    SET @usu_id = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_UpdateUsuario
    @usu_id INT,
    @usu_google_id NVARCHAR(100) = NULL,
    @usu_email NVARCHAR(100),
    @usu_rol NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE t_usuario SET
        usu_google_id = @usu_google_id,
        usu_email = @usu_email,
        usu_rol = @usu_rol
    WHERE usu_id = @usu_id;
END
GO

CREATE OR ALTER PROCEDURE dbo.GetInfoClase
    @SearchParam NVARCHAR(255),
    @SearchType NVARCHAR(50) = NULL  -- Puede ser 'docente', 'clase', 'asignatura', 'salon'
AS
BEGIN
    SET NOCOUNT ON;

    IF @SearchParam IS NULL OR LTRIM(RTRIM(@SearchParam)) = ''
    BEGIN
        RAISERROR('El parámetro de búsqueda no puede estar vacío.', 16, 1);
        RETURN;
    END

    IF @SearchType IS NULL OR @SearchType NOT IN ('docente', 'clase', 'asignatura', 'salon')
    BEGIN
        RAISERROR('El tipo de búsqueda debe ser "docente", "clase", "asignatura" o "salon".', 16, 1);
        RETURN;
    END

    IF @SearchType = 'clase' AND ISNUMERIC(@SearchParam) = 0
    BEGIN
        RAISERROR('Para búsquedas por clase, debe proporcionar un valor numérico válido.', 16, 1);
        RETURN;
    END

    SELECT 
        hor.hor_dia AS Dia,
        hor.hor_hora_inicio AS HoraInicio,
        hor.hor_hora_fin AS HoraFin,
        hor.hor_fecha_inicio AS FechaInicio,
        hor.hor_fecha_fin AS FechaFin,
        sal.sal_codigo AS SalonCodigo,
        sal.sal_nombre AS SalonNombre,
        blo.blo_nombre AS BloqueNombre,
        edi.edi_nombre AS EdificioNombre,
        sed.sed_nombre AS SedeNombre,
        asi.asi_nombre AS AsignaturaNombre,
        asi.asi_docente_nombre AS DocenteNombre,
        cam.cam_fecha_inicio AS CambioFechaInicio,
        cam.cam_fecha_fin AS CambioFechaFin,
        cam.cam_motivo AS CambioMotivo,
        cam.cam_nuevo_sal_id AS NuevoSalonID,
        nsal.sal_nombre AS NuevoSalonNombre,
        nblo.blo_nombre AS NuevoBloqueNombre,
        nedi.edi_nombre AS NuevoEdificioNombre,
        nsed.sed_nombre AS NuevaSedeNombre,
        sal.sal_imagen_url AS SalonImagenUrl,
        edi.edi_imagen_url AS EdificioImagenUrl,
        (
            SELECT rut_latitud AS Latitud, rut_longitud AS Longitud
            FROM t_ruta_edificio
            WHERE rut_edi_id = edi.edi_id
            ORDER BY rut_orden
            FOR JSON PATH
        ) AS RutaEdificio
    FROM 
        t_horario hor
    INNER JOIN 
        t_asignatura asi ON hor.hor_asi_id = asi.asi_id
    INNER JOIN 
        t_salon sal ON hor.hor_sal_id = sal.sal_id
    INNER JOIN 
        t_bloque blo ON sal.sal_blo_id = blo.blo_id
    INNER JOIN 
        t_edificio edi ON blo.blo_edi_id = edi.edi_id
    INNER JOIN 
        t_sede sed ON edi.edi_sed_id = sed.sed_id
    LEFT JOIN 
        t_cambio_aula cam ON hor.hor_id = cam.cam_hor_id
    LEFT JOIN 
        t_salon nsal ON cam.cam_nuevo_sal_id = nsal.sal_id
    LEFT JOIN 
        t_bloque nblo ON nsal.sal_blo_id = nblo.blo_id
    LEFT JOIN 
        t_edificio nedi ON nblo.blo_edi_id = nedi.edi_id
    LEFT JOIN 
        t_sede nsed ON nedi.edi_sed_id = nsed.sed_id
    WHERE 
        CASE @SearchType
            WHEN 'docente' THEN 
                CASE WHEN asi.asi_docente_nombre LIKE '%' + @SearchParam + '%' THEN 1 ELSE 0 END
            WHEN 'clase' THEN 
                CASE WHEN asi.asi_id = CAST(@SearchParam AS INT) THEN 1 ELSE 0 END
            WHEN 'asignatura' THEN 
                CASE WHEN asi.asi_nombre LIKE '%' + @SearchParam + '%' THEN 1 ELSE 0 END
            WHEN 'salon' THEN 
                CASE WHEN sal.sal_nombre LIKE '%' + @SearchParam + '%' THEN 1 ELSE 0 END
            ELSE 0
        END = 1;

    IF @@ROWCOUNT = 0
    BEGIN
        DECLARE @Mensaje NVARCHAR(255);
        SET @Mensaje = 'No se encontraron resultados para ' + @SearchType + ': ' + @SearchParam;
        PRINT @Mensaje;
    END
END
GO
/*
EXEC dbo.GetInfoClase @SearchParam = 'Willington', @SearchType = 'docente';
EXEC dbo.GetInfoClase @SearchParam = '3', @SearchType = 'clase';
EXEC dbo.GetInfoClase @SearchParam = 'Ingles V', @SearchType = 'asignatura';
EXEC dbo.GetInfoClase @SearchParam = '106', @SearchType = 'salon';
*/

CREATE OR ALTER PROCEDURE dbo.GetScheduleByStudentCode
    @StudentCode NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.usu_nombre AS StudentName,
        u.usu_codigo_estudiante AS StudentCode,
        a.asi_nombre AS SubjectName,
        a.asi_docente_nombre AS TeacherName,
        h.hor_dia AS Day,
        h.hor_hora_inicio AS StartTime,
        h.hor_hora_fin AS EndTime,
        h.hor_fecha_inicio AS StartDate,
        h.hor_fecha_fin AS EndDate,
        s.sal_nombre AS Room,
        b.blo_nombre AS Block,
        e.edi_nombre AS Building,
        d.sed_nombre AS Campus,
        ca.cam_fecha_inicio AS ChangeStartDate,
        ca.cam_fecha_fin AS ChangeEndDate,
        ca.cam_motivo AS ChangeReason
    FROM 
        t_usuario u
    INNER JOIN 
        t_planificacion_asignatura p ON u.usu_codigo_estudiante = p.plan_codigo_estudiante
    INNER JOIN 
        t_asignatura a ON p.plan_asi_id = a.asi_id
    LEFT JOIN 
        t_horario h ON a.asi_id = h.hor_asi_id
    LEFT JOIN 
        t_salon s ON h.hor_sal_id = s.sal_id
    LEFT JOIN 
        t_bloque b ON s.sal_blo_id = b.blo_id
    LEFT JOIN 
        t_edificio e ON b.blo_edi_id = e.edi_id
    LEFT JOIN 
        t_sede d ON e.edi_sed_id = d.sed_id
    LEFT JOIN 
        t_cambio_aula ca ON h.hor_id = ca.cam_hor_id
    WHERE 
        u.usu_codigo_estudiante = @StudentCode
    ORDER BY 
        CASE h.hor_dia
            WHEN 'Lunes' THEN 1
            WHEN 'Martes' THEN 2
            WHEN 'Miércoles' THEN 3
            WHEN 'Miercoles' THEN 3
            WHEN 'Jueves' THEN 4
            WHEN 'Viernes' THEN 5
            WHEN 'Sábado' THEN 6
            WHEN 'Sabado' THEN 6
            WHEN 'Domingo' THEN 7
            ELSE 8
        END,
        h.hor_hora_inicio;
END
GO

CREATE OR ALTER PROCEDURE sp_GenerarNotificacionesCambioAula
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO t_notificacion (not_usu_id, not_mensaje, not_tipo)
    SELECT 
        u.usu_id,
        'Cambio de aula para la asignatura "' + a.asi_nombre + '" dictada por ' + a.asi_docente_nombre + 
        '. Nuevo salón: ' + ISNULL(nsal.sal_nombre, 'N/A') + ' (' + 
        ISNULL(nblo.blo_nombre, 'Bloque') + ', ' + ISNULL(nedi.edi_nombre, 'Edificio') + ', ' + ISNULL(nsed.sed_nombre, 'Sede') + ')',
        'cambio_aula'
    FROM t_usuario u
    INNER JOIN t_planificacion_asignatura p ON u.usu_codigo_estudiante = p.plan_codigo_estudiante
    INNER JOIN t_asignatura a ON p.plan_asi_id = a.asi_id
    INNER JOIN t_horario h ON a.asi_id = h.hor_asi_id
    INNER JOIN t_cambio_aula cam ON cam.cam_hor_id = h.hor_id
    LEFT JOIN t_salon nsal ON cam.cam_nuevo_sal_id = nsal.sal_id
    LEFT JOIN t_bloque nblo ON nsal.sal_blo_id = nblo.blo_id
    LEFT JOIN t_edificio nedi ON nblo.blo_edi_id = nedi.edi_id
    LEFT JOIN t_sede nsed ON nedi.edi_sed_id = nsed.sed_id
    WHERE 
        CAST(GETDATE() AS DATE) BETWEEN cam.cam_fecha_inicio AND cam.cam_fecha_fin
        AND NOT EXISTS (
            SELECT 1 FROM t_notificacion n 
            WHERE n.not_usu_id = u.usu_id 
              AND n.not_mensaje LIKE '%' + a.asi_nombre + '%'
              AND n.not_leida = 0
        );
END
GO