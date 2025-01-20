<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "piscifactoria";

// Crear conexión
$conn = new mysqli($servername, $username, $password, $dbname);

// Verificar conexión
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Recibir datos del formulario



// Consultar base de datos
$sql =  "INSERT INTO games (total_deaths,total_powerups,total_ammo_gathered,winner,loser_a,loser_b,loser_c) VALUES(1,1,1,3,3,3,3)";

$result = $conn->query($sql);

    if ($result) {
        $last_id = $conn->insert_id;
  echo "New record created successfully. Last inserted ID is: " . $last_id;
    } else {
        echo json_encode(array("status" => "failed", "message" => "Error updating games: " . $conn->error));
    }


$conn->close();
?>