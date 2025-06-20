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

@when('Ingreso con una cuenta de Google no autorizada')
def step_login_with_invalid_google(context):
    wait = WebDriverWait(context.driver, int(os.getenv("WAIT_TIMEOUT", 20)))
    
    context.driver.find_element(By.CLASS_NAME, "btn-google").click()

    email_input = wait.until(EC.visibility_of_element_located((By.ID, "identifierId")))
    email_input.send_keys(os.getenv("GOOGLE_EMAIL_ALT"))
    context.driver.find_element(By.ID, "identifierNext").click()

    passwd_input = wait.until(EC.element_to_be_clickable((By.CSS_SELECTOR, "input[type='password'][name='Passwd']")))
    for char in os.getenv("GOOGLE_PASSWORD_ALT"):
        passwd_input.send_keys(char)
        time.sleep(0.1)
    context.driver.find_element(By.ID, "passwordNext").click()

    print("Intento de login inválido realizado.")

@then('Debería ver un mensaje de error o denegación de acceso')
def step_verify_error_message(context):
    try:
        wait = WebDriverWait(context.driver, 20)

        # Esperar a que la URL cambie a la página de error
        wait.until(EC.url_contains("/Account/Error"))
        print("✅ Redirigido a la página de error.")

        # Esperar a que aparezca el mensaje de error
        error_element = wait.until(
            EC.visibility_of_element_located(
                (By.XPATH, "//h4[contains(text(), 'Ha ocurrido un error')]")
            )
        )

        assert "Ha ocurrido un error" in error_element.text
        print(f"✅ Mensaje de error detectado: {error_element.text}")

        time.sleep(3)  # Pausa para ver el mensaje antes de cerrar

    except Exception as e:
        raise Exception("❌ No se detectó el mensaje de error esperado.") from e

    finally:
        context.driver.quit()