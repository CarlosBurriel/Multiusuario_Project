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
$inputTotal_Deaths = $_POST['deaths'];
$inputTotal_PowerUps = $_POST['powerups'];
$inputTotal_Ammo_Gathered = $_POST['ammo'];
$inputWinner = $_POST['winner'];
$inputLoser_a = $_POST['loserA'];
$inputLoser_b = $_POST['loserB'];
$inputLoser_c = $_POST['loserC'];


// Consultar base de datos
$sql =  "UPDATE games SET total_deaths='$inputTotal_Deaths',total_powerups='$inputTotal_PowerUps',total_ammo_gathered='$inputTotal_Ammo_Gathered',winner=(SELECT account_id FROM accounts WHERE username= '$inputWinner'),loser_a=(SELECT account_id FROM accounts WHERE username= '$inputLoser_a'),loser_b=(SELECT account_id FROM accounts WHERE username= '$inputLoser_b'),loser_c=(SELECT account_id FROM accounts WHERE username= '$inputLoser_c') WHERE games_id= '$inputGameID' ";

$result = $conn->query($sql);

    if ($result) {
        echo json_encode(array("status" => "success", "message" => "Games updated"));
    } else {
        echo json_encode(array("status" => "failed", "message" => "Error updating games: " . $conn->error));
    }


$conn->close();
?>