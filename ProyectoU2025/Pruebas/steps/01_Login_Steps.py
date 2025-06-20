# steps/login_steps.py

import os
import time
from dotenv import load_dotenv
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.common.exceptions import TimeoutException
from behave import given, when, then

load_dotenv()

def setup_driver():
    edge_options = webdriver.EdgeOptions()
    edge_options.add_argument("--disable-blink-features=AutomationControlled")
    edge_options.add_experimental_option("excludeSwitches", ["enable-automation"])
    edge_options.add_experimental_option("useAutomationExtension", False)

    driver = webdriver.Edge(options=edge_options)
    driver.maximize_window()
    return driver

@given('Estoy en la página de inicio')
def step_open_login_page(context):
    context.driver = setup_driver()
    url = os.getenv("BASE_URL")
    context.driver.get(url)

    WebDriverWait(context.driver, int(os.getenv("WAIT_TIMEOUT", 20))).until(
        EC.presence_of_element_located((By.CLASS_NAME, "btn-google"))
    )

@when('Ingreso con mi cuenta de Google')
def step_login_with_google(context):
    wait = WebDriverWait(context.driver, int(os.getenv("WAIT_TIMEOUT", 20)))
    
    context.driver.find_element(By.CLASS_NAME, "btn-google").click()

    email_input = wait.until(EC.visibility_of_element_located((By.ID, "identifierId")))
    email_input.send_keys(os.getenv("GOOGLE_EMAIL"))
    context.driver.find_element(By.ID, "identifierNext").click()

    passwd_input = wait.until(EC.element_to_be_clickable((By.CSS_SELECTOR, "input[type='password'][name='Passwd']")))
    for char in os.getenv("GOOGLE_PASSWORD"):
        passwd_input.send_keys(char)
        time.sleep(0.1)
    context.driver.find_element(By.ID, "passwordNext").click()

    try:
        passkey_button = wait.until(
            EC.element_to_be_clickable((By.XPATH, "//button[contains(text(), 'Usar tu llave de acceso') or contains(text(), 'Passkey')]"))
        )
        passkey_button.click()
        print("Se detectó y manejó paso de llave de acceso (Passkey).")
    except TimeoutException:
        print("No se mostró opción de llave de acceso. Continuando...")

    wait.until(EC.url_contains(os.getenv("BASE_URL")))
    print("Login completado y redirigido correctamente.")

@then('Debería ver la página de Dashboard')
def step_verify_dashboard(context):
    try:
        wait = WebDriverWait(context.driver, 15)

        # Verifica que el título de bienvenida esté presente
        h1_bienvenida = wait.until(EC.presence_of_element_located(
            (By.XPATH, "//h1[contains(text(), '¡Bienvenido') or contains(text(), '¡Bienvenida')]")
        ))

        assert "Bienvenid" in h1_bienvenida.text  # Puede ser Bienvenido o Bienvenida
        print(f"Login exitoso. Redirigido al Dashboard con mensaje: {h1_bienvenida.text}")
    except Exception as e:
        raise Exception("No se encontró el mensaje de bienvenida en el Dashboard.") from e
    finally:
        context.driver.quit()
