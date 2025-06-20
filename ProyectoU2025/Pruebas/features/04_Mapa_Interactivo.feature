Feature: Mapa interactivo del campus
  Como usuario, quiero buscar el salón de un docente desde el mapa del campus
  para ubicarme rápidamente en la universidad.
  
  Scenario: Buscar salón por nombre de docente en el mapa del campus
    Given Estoy en la página de inicio
    When Ingreso con mi cuenta de Google
    And Navego al módulo "Mapa del Campus"
    And Selecciono la opción "Docente" en el mapa del campus
    And Escribo "willi" en la barra de búsqueda del mapa del campus
    And Presiono el botón de búsqueda en el mapa del campus
    Then Debería ver el salón asignado y el mapa interactivo con la ruta del docente

