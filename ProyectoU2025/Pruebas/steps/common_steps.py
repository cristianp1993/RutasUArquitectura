import os
from selenium import webdriver

def setup_driver():
    edge_options = webdriver.EdgeOptions()
    edge_options.add_argument("--disable-blink-features=AutomationControlled")
    edge_options.add_experimental_option("excludeSwitches", ["enable-automation"])
    edge_options.add_experimental_option("useAutomationExtension", False)

    driver = webdriver.Edge(options=edge_options)
    driver.maximize_window()
    return driver