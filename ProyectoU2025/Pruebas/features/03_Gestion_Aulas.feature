Feature: Búsqueda de salones por docente

  Scenario: Buscar salón por nombre de docente
    Given Estoy en la página de inicio
    When Ingreso con mi cuenta de Google
    And Navego al módulo "Buscar Salón"
    And Selecciono la opción "Docente"
    And Escribo "willi" en la barra de búsqueda
    And Presiono el botón de búsqueda
    Then Debería ver el salón asignado al docente
