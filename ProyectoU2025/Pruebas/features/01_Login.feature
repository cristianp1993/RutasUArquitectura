Feature: Login en CampusNav

  Scenario: Login exitoso con credenciales válidas
    Given Estoy en la página de inicio
    When Ingreso con mi cuenta de Google
    Then Debería ver la página de Dashboard