from behave import given, when, then
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
import os

@given('No estoy autenticado')
def step_no_auth(context):
    context.driver = webdriver.Edge()
    context.driver.delete_all_cookies()

@when('Navego directamente al Dashboard')
def step_direct_dashboard(context):
    context.driver.get("https://localhost:7188/Profile/Index")

@then('Debería ser redirigido al dashboard de invitado')
def step_redirect_login(context):
    wait = WebDriverWait(context.driver, 10)
    wait.until(EC.url_contains(os.getenv("BASE_URL")))
    
    # Espera unos segundos para que puedas ver la página antes de cerrar
    import time
    time.sleep(5)
    context.driver.quit()
