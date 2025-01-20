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
$inputUsername = $_POST['username'];

// Consultar base de datos
$sql =  "UPDATE account_stats SET deaths= deaths+ 1  WHERE player_id = (SELECT account_id FROM accounts WHERE username= '$inputUsername')";

$result = $conn->query($sql);

    if ($result) {
        echo json_encode(array("status" => "success", "message" => "Death updated"));
    } else {
        echo json_encode(array("status" => "failed", "message" => "Error updating death: " . $conn->error));
    }


$conn->close();
?>