<?php
$servername = "localhost";
<?php
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
$sql = "SELECT * FROM games WHERE username='$inputUsername'";

$result = $conn->query($sql);
if ($result->num_rows > 0) {
    // Si ya existe el username, actualizamos la columna winner
    $updateSql = "UPDATE games SET loser_a='$inputUsername' WHERE username='$inputUsername'";

    if ($conn->query($updateSql) === TRUE) {
        echo json_encode(array("status" => "success", "message" => "Winner updated"));
    } else {
        echo json_encode(array("status" => "failed", "message" => "Error updating winner: " . $conn->error));
    }
} else {
    // Si no existe el username, puedes elegir cómo manejar el caso (por ejemplo, insertar un nuevo registro)
    echo json_encode(array("status" => "failed", "message" => "No game found with this username"));
}

$conn->close();
?>