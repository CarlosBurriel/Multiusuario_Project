<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "unity_login";

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
$sql = "INSERT INTO users(username, password) values ('$inputUsername','$inputPassword')";
$result = $conn->query($sql);

if ($result) {
    echo json_encode(array("status" => "success"));
} else {
    echo json_encode(array("status" => "failed"));
}

$conn->close();
?>
