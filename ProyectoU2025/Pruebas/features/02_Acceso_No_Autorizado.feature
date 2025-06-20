Feature: Acceso sin login

  Scenario: Intentar acceder al Dashboard sin estar logueado
    Given No estoy autenticado
    When Navego directamente al Dashboard
    Then Debería ser redirigido al dashboard de invitado
