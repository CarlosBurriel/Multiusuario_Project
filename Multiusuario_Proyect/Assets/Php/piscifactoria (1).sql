-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 12-01-2025 a las 17:19:40
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `piscifactoria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `accounts`
--

CREATE TABLE `accounts` (
  `account_id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `creation_date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `accounts`
--

INSERT INTO `accounts` (`account_id`, `username`, `password`, `creation_date`) VALUES
(3, 'furrito77', 'pingadefurro', '0000-00-00'),
(4, 'CarlosBurriel', 'PiscifactoriaDev', '0000-00-00'),
(5, 'Dieguito Maradona', 'pez', '0000-00-00'),
(6, 'Villareal', 'Furbo', '0000-00-00'),
(7, 'pluto', 'pluton', '0000-00-00');

--
-- Disparadores `accounts`
--
DELIMITER $$
CREATE TRIGGER `after_accounts_insert` AFTER INSERT ON `accounts` FOR EACH ROW BEGIN
    INSERT INTO account_stats (player_id, deaths, kills, powerups_gathered, ammo_gathered, wins, defeats, games_played)
    VALUES (NEW.account_id, 0, 0, 0, 0, 0, 0, 0);
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `account_stats`
--

CREATE TABLE `account_stats` (
  `player_id` int(11) NOT NULL,
  `deaths` int(11) DEFAULT NULL,
  `kills` int(11) DEFAULT NULL,
  `powerups_gathered` int(11) DEFAULT NULL,
  `ammo_gathered` int(11) DEFAULT NULL,
  `wins` int(11) DEFAULT NULL,
  `defeats` int(11) DEFAULT NULL,
  `games_played` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `account_stats`
--

INSERT INTO `account_stats` (`player_id`, `deaths`, `kills`, `powerups_gathered`, `ammo_gathered`, `wins`, `defeats`, `games_played`) VALUES
(3, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(4, 0, 0, 0, 0, 1, 1, 2),
(5, 0, 0, 0, 0, 0, 2, 2),
(6, 0, 0, 0, 0, 0, 2, 2),
(7, 0, 0, 0, 0, 0, 1, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `deaths_table`
--

CREATE TABLE `deaths_table` (
  `death_id` int(11) NOT NULL,
  `id_game` int(11) NOT NULL,
  `death_position_x` int(11) NOT NULL,
  `death_position_z` int(11) NOT NULL,
  `killer_id` int(11) NOT NULL,
  `victim_id` int(11) NOT NULL,
  `time_of_death` datetime NOT NULL,
  `weapon` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `games`
--

CREATE TABLE `games` (
  `games_id` int(11) NOT NULL,
  `total_deaths` int(11) DEFAULT NULL,
  `total_powerups` int(11) DEFAULT NULL,
  `total_ammo_gathered` int(11) DEFAULT NULL,
  `winner` int(11) NOT NULL,
  `loser_a` int(11) NOT NULL,
  `loser_b` int(11) NOT NULL,
  `loser_c` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `games`
--

INSERT INTO `games` (`games_id`, `total_deaths`, `total_powerups`, `total_ammo_gathered`, `winner`, `loser_a`, `loser_b`, `loser_c`) VALUES
(6, NULL, NULL, NULL, 3, 4, 5, 6),
(7, NULL, NULL, NULL, 4, 5, 6, 7);

--
-- Disparadores `games`
--
DELIMITER $$
CREATE TRIGGER `after_game_insert` AFTER INSERT ON `games` FOR EACH ROW BEGIN
    -- Actualizar estadísticas del ganador
    UPDATE account_stats
    SET wins = wins + 1, games_played = games_played + 1
    WHERE player_id = NEW.winner;

    -- Actualizar estadísticas de los perdedores
    UPDATE account_stats
    SET defeats = defeats + 1, games_played = games_played + 1
    WHERE player_id IN (NEW.loser_a, NEW.loser_b, NEW.loser_c);
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `games_history`
--

CREATE TABLE `games_history` (
  `id_game` int(11) NOT NULL,
  `id_player` int(11) NOT NULL,
  `deaths` int(11) DEFAULT NULL,
  `kills` int(11) DEFAULT NULL,
  `ammo_gathered` int(11) DEFAULT NULL,
  `powerup_used` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`account_id`);

--
-- Indices de la tabla `account_stats`
--
ALTER TABLE `account_stats`
  ADD PRIMARY KEY (`player_id`);

--
-- Indices de la tabla `deaths_table`
--
ALTER TABLE `deaths_table`
  ADD PRIMARY KEY (`death_id`),
  ADD KEY `id_game` (`id_game`),
  ADD KEY `killer_id` (`killer_id`),
  ADD KEY `victim_id` (`victim_id`);

--
-- Indices de la tabla `games`
--
ALTER TABLE `games`
  ADD PRIMARY KEY (`games_id`),
  ADD KEY `winner` (`winner`),
  ADD KEY `loser_a` (`loser_a`),
  ADD KEY `loser_b` (`loser_b`),
  ADD KEY `loser_c` (`loser_c`);

--
-- Indices de la tabla `games_history`
--
ALTER TABLE `games_history`
  ADD KEY `id_game` (`id_game`),
  ADD KEY `id_player` (`id_player`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `accounts`
--
ALTER TABLE `accounts`
  MODIFY `account_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `deaths_table`
--
ALTER TABLE `deaths_table`
  MODIFY `death_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `games`
--
ALTER TABLE `games`
  MODIFY `games_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `account_stats`
--
ALTER TABLE `account_stats`
  ADD CONSTRAINT `account_stats_ibfk_1` FOREIGN KEY (`player_id`) REFERENCES `accounts` (`account_id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `deaths_table`
--
ALTER TABLE `deaths_table`
  ADD CONSTRAINT `deaths_table_ibfk_1` FOREIGN KEY (`id_game`) REFERENCES `games` (`games_id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `deaths_table_ibfk_2` FOREIGN KEY (`killer_id`) REFERENCES `account_stats` (`player_id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `deaths_table_ibfk_3` FOREIGN KEY (`victim_id`) REFERENCES `account_stats` (`player_id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `games`
--
ALTER TABLE `games`
  ADD CONSTRAINT `games_ibfk_1` FOREIGN KEY (`winner`) REFERENCES `account_stats` (`player_id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `games_ibfk_2` FOREIGN KEY (`loser_a`) REFERENCES `account_stats` (`player_id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `games_ibfk_3` FOREIGN KEY (`loser_b`) REFERENCES `account_stats` (`player_id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `games_ibfk_4` FOREIGN KEY (`loser_c`) REFERENCES `account_stats` (`player_id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `games_history`
--
ALTER TABLE `games_history`
  ADD CONSTRAINT `games_history_ibfk_1` FOREIGN KEY (`id_game`) REFERENCES `games` (`games_id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `games_history_ibfk_2` FOREIGN KEY (`id_player`) REFERENCES `account_stats` (`player_id`) ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
