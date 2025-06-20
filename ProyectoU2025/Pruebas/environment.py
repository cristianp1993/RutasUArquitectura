from steps.common_steps import setup_driver

def before_scenario(context, scenario):
    context.driver = setup_driver()

def after_scenario(context, scenario):
    if hasattr(context, 'driver'):
        try:
            context.driver.quit()
        except:
            pass
