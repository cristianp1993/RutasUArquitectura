import time
from behave import when, then
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import Select
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.support.ui import WebDriverWait

@when('Navego al módulo "Mapa del Campus"')
def step_go_to_campus_map(context):
    wait = WebDriverWait(context.driver, 15)
    ver_mapa_btn = wait.until(
        EC.element_to_be_clickable((By.XPATH, "//a[contains(text(),'Ver mapa') and @href='/Search']"))
    )
    ver_mapa_btn.click()
    print("✅ Clic en 'Ver mapa' del módulo 'Mapa del Campus' en el dashboard realizado.")

@when('Selecciono la opción "Docente" en el mapa del campus')
def step_select_teacher_option_map(context):
    wait = WebDriverWait(context.driver, 10)
    dropdown_element = wait.until(
        EC.presence_of_element_located((By.ID, "SelecSearcht"))
    )
    dropdown = Select(dropdown_element)
    dropdown.select_by_visible_text("Docente")
    print("✅ Opción 'Docente' seleccionada en el mapa del campus.")

@when('Escribo "willi" en la barra de búsqueda del mapa del campus')
def step_enter_teacher_name_map(context):
    wait = WebDriverWait(context.driver, 10)
    search_input = wait.until(
        EC.presence_of_element_located((By.ID, "ValueSearch"))
    )
    search_input.clear()
    search_input.send_keys("willi")
    print("✅ Nombre 'willi' ingresado en el campo de búsqueda del mapa del campus.")

@when('Presiono el botón de búsqueda en el mapa del campus')
def step_click_search_button_map(context):
    wait = WebDriverWait(context.driver, 10)
    search_button = wait.until(
        EC.element_to_be_clickable((By.ID, "btn-buscar-info"))
    )
    search_button.click()
    print("✅ Botón 'BUSCAR' presionado en el mapa del campus.")

@then('Debería ver el salón asignado y el mapa interactivo con la ruta del docente')
def step_see_assigned_classroom_and_map(context):
    wait = WebDriverWait(context.driver, 10)

    # Verifica el mensaje de éxito
    result_alert = wait.until(
        EC.presence_of_element_located((By.CLASS_NAME, "alert-success"))
    )
    assert "resultado(s) encontrados" in result_alert.text.lower()
    print("✅ Mensaje de éxito mostrado.")

    # Verifica que haya tarjetas con resultados
    cards = wait.until(
        EC.presence_of_all_elements_located((By.CLASS_NAME, "alert-info"))
    )
    assert len(cards) > 0, "No se encontraron resultados informativos."
    print(f"✅ Se mostraron {len(cards)} resultado(s):")
    for card in cards:
        print("📝", card.text.strip())

    # Verifica que el mapa esté visible
    map_element = wait.until(
        EC.visibility_of_element_located((By.ID, "my_map"))
    )
    assert map_element.is_displayed(), "El mapa no está visible."
    assert map_element.size["height"] > 0 and map_element.size["width"] > 0, "El mapa no tiene tamaño visible."
    print("🗺️ Mapa visible y cargado correctamente.")

    # ✅ Clic en botón de ruta de la primera card
    route_button = wait.until(
        EC.element_to_be_clickable((By.CSS_SELECTOR, ".alert-info .btn-outline-success"))
    )
    route_button.click()
    print("🚶 Clic en el botón de ruta del primer resultado realizado.")

    # ✅ Verifica que aparezca la leyenda del recorrido
    legend_title = wait.until(
        EC.presence_of_element_located((By.XPATH, "//div[contains(@class,'leaflet-control')]//h4[contains(text(),'Leyenda del Recorrido')]"))
    )
    assert legend_title.is_displayed(), "La leyenda del recorrido no está visible en el mapa."
    print("📌 Leyenda del recorrido mostrada correctamente.")

    time.sleep(5)
