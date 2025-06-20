Feature: Login inválido en CampusNav

  Scenario: Intento de login con un correo que no pertenece a @ucaldas.edu.co
    Given Estoy en la página de inicio
    When Ingreso con una cuenta de Google no autorizada
    Then Debería ver un mensaje de error o denegación de acceso
