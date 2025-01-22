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
$inputGameID = $_POST['gameid'];
$inputDeathPositionX = $_POST['deathpositionx'];
$inputDeathPositionZ = $_POST['deathpositionz'];
$inputKillerID = $_POST['killerid'];
$inputVictimID = $_POST['victimid'];
$inputTimeOfDeath = $_POST['timeofdeath'];





// Consultar base de datos
$sql =  "INSERT INTO deaths_table (id_game,death_position_x,death_position_z,killer_id,victim_id,time_of_death) VALUES('$inputGameID','$inputDeathPositionX','$inputDeathPositionZ',(SELECT account_id FROM accounts WHERE username= '$inputKillerID'),(SELECT account_id FROM accounts WHERE username= '$inputVictimID '),'$inputTimeOfDeath')";

$result = $conn->query($sql);

    if ($result) {
        echo json_encode(array("status" => "success", "message" => "Kills updated"));
    } else {
        echo json_encode(array("status" => "failed", "message" => "Error updating Kills: " . $conn->error));
    }


$conn->close();
?>