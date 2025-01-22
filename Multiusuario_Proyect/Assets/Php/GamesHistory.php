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
$inputIDPlayer = $_POST['username'];
$inputDeaths = $_POST['deaths'];
$inputKills = $_POST['kills'];
$inputAmmoGathered = $_POST['ammo'];
$inputPowerUpsGathered = $_POST['powerups'];





// Consultar base de datos
$sql =  "INSERT INTO games_history (id_game,id_player,deaths,kills,ammo_gathered,powerup_used) VALUES('$inputGameID',(SELECT account_id FROM accounts WHERE username= '$inputIDPlayer'),'$inputDeaths','$inputKills','$inputAmmoGathered','$inputPowerUpsGathered')";

$result = $conn->query($sql);

    if ($result) {
        echo json_encode(array("status" => "success", "message" => "Kills updated"));
    } else {
        echo json_encode(array("status" => "failed", "message" => "Error updating Kills: " . $conn->error));
    }


$conn->close();
?>