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
$sql = "INSERT INTO accounts(username, password) values ('$inputUsername','$inputPassword')";
try {
    $result = $conn->query($sql);
    echo json_encode(array("status" => "success"));
} catch (mysqli_sql_exception $e) {
    if ($e->getCode() == 1062) {
        echo json_encode(array("status" => "duplicate"));
    } 
    else {
        echo json_encode(array("status" => "failed"));
    }
}

$conn->close();
