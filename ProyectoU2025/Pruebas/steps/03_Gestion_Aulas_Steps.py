import time
from behave import given, when, then
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import Select
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.support.ui import WebDriverWait

@when('Navego al módulo "Buscar Salón"')
def step_go_to_search_room(context):
    wait = WebDriverWait(context.driver, 15)
    buscar_salon_btn = wait.until(
        EC.element_to_be_clickable((By.XPATH, "//a[contains(., 'Buscar Salón')]"))
    )
    buscar_salon_btn.click()
    print("✅ Clic en 'Buscar Salón' realizado.")

@when('Selecciono la opción "Docente"')
def step_select_teacher_option(context):
    wait = WebDriverWait(context.driver, 10)
    dropdown_element = wait.until(
        EC.presence_of_element_located((By.ID, "SelecSearcht"))
    )
    dropdown = Select(dropdown_element)
    dropdown.select_by_visible_text("Docente")
    print("✅ Opción 'Docente' seleccionada.")

@when('Escribo "willi" en la barra de búsqueda')
def step_enter_teacher_name(context):
    wait = WebDriverWait(context.driver, 10)
    search_input = wait.until(
        EC.presence_of_element_located((By.ID, "ValueSearch"))
    )
    search_input.clear()
    search_input.send_keys("willi")
    print("✅ Nombre 'willi' ingresado en el campo correcto.")

@when('Presiono el botón de búsqueda')
def step_click_search_button(context):
    wait = WebDriverWait(context.driver, 10)
    search_button = wait.until(
        EC.element_to_be_clickable((By.ID, "btn-buscar-info"))
    )
    search_button.click()
    print("✅ Botón 'BUSCAR' presionado.")

@then('Debería ver el salón asignado al docente')
def step_see_assigned_classroom(context):
    wait = WebDriverWait(context.driver, 10)

    # Espera a que aparezca el contenedor del resultado exitoso
    result_alert = wait.until(
        EC.presence_of_element_located((By.CLASS_NAME, "alert-success"))
    )
    assert "resultado(s) encontrados" in result_alert.text.lower()

    # Espera a que aparezca al menos una tarjeta informativa del docente
    cards = wait.until(
        EC.presence_of_all_elements_located((By.CLASS_NAME, "alert-info"))
    )
    assert len(cards) > 0, "No se encontraron resultados informativos."

    print(f"✅ Se mostraron {len(cards)} resultado(s):")
    for card in cards:
        print("📝", card.text.strip())
       
    time.sleep(5)