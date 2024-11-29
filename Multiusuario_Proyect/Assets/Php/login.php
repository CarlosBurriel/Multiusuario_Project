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
$inputPassword = $_POST['password'];

// Consultar base de datos
$sql = "SELECT * FROM accounts WHERE username='$inputUsername' AND password='$inputPassword'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    echo json_encode(array("status" => "success"));
} else {
    echo json_encode(array("status" => "failed"));
}

$conn->close();
?>
