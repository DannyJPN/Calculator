document.addEventListener('DOMContentLoaded', async function () {
    const operators = ["+", "*", "-", "/"];
    const operator_regex = /[+\-*\/]/;
    const dec_dot_regex = /[\.]/;
    const number_regex = /[0-9]$/;
    const records_count = 10;
    let records = await loadLastCalculations(records_count);
    updateHistory(records);

    let digit_buttons = document.getElementsByClassName('btn-input-digit');
    for (let i = 0; i < digit_buttons.length; i++) {
        digit_buttons[i].addEventListener('click', function () {
            let caller = this;
            if (caller instanceof HTMLElement) {
                let textbox = document.getElementById('calc-display');
                textbox.value += caller.innerText.trim();
            }
        });
    }

    let operator_buttons = document.getElementsByClassName('btn-input-operator');
    for (let i = 0; i < operator_buttons.length; i++) {
        operator_buttons[i].addEventListener('click', function () {
            let caller = this;
            if (caller instanceof HTMLElement) {
                let textbox = document.getElementById('calc-display');
                if (!operator_regex.test(textbox.value)) {
                    textbox.value += caller.innerText.trim();
                }
            }
        });
    }

    let decimaldigit_buttons = document.getElementsByClassName('btn-input-decimalpoint');
    for (let i = 0; i < decimaldigit_buttons.length; i++) {
        decimaldigit_buttons[i].addEventListener('click', function () {
            let caller = this;
            if (caller instanceof HTMLElement) {
                let textbox = document.getElementById('calc-display');
                let numbers = textbox.value.split(operator_regex);
                let lastnum = numbers[numbers.length - 1];
                if (!dec_dot_regex.test(lastnum) && number_regex.test(lastnum)) {
                    textbox.value += caller.innerText.trim();
                }
            }
        });
    }

    let clear_buttons = document.getElementsByClassName('btn-input-clear');
    for (let i = 0; i < clear_buttons.length; i++) {
        clear_buttons[i].addEventListener('click', function () {
            let caller = this;
            if (caller instanceof HTMLElement) {
                let textbox = document.getElementById('calc-display');
                textbox.value = "";
            }
        });
    }

    let calc_button = document.getElementById('btn-calculate');
    calc_button.addEventListener('click', async function () {
        let caller = this;
        let errorContainer = document.getElementById('error-container');
        errorContainer.innerText = "";
        if (caller instanceof HTMLElement) {
            let textbox = document.getElementById('calc-display');
            let expression = textbox.value;
            if (expression == "" || expression == undefined || expression == null) {
                displayError("Empty calculation expression.");
                return;
            }
            if (!isValidExpression(expression)) {
                displayError("Invalid expression format.");
                return;
            }
            let result = await calculate_expression(expression);
            if (result !== "Error") {
                if (expression !== result) {
                    let saving_result = await save_result(expression, result);
                    if (saving_result) {
                        textbox.value = result;
                        let records = await loadLastCalculations(records_count);
                        updateHistory(records);
                    } else {
                        console.error("Failed to save the calculation record.");
                    }
                } else {
                    textbox.value = result;
                }
            } else {
                console.error("Invalid expression.", expression);
            }
        }
    });

    function isValidExpression(expression) {
        expression = expression.replace(" ", "");
        for (let op of operators) {
            let parts = expression.split(op);
            if (parts.length == 2) {
                if (!isNaN(parseFloat(parts[0])) && !isNaN(parseFloat(parts[1]))) {
                    return true;
                }
            }
        }
        return !isNaN(parseFloat(expression));
    }

    async function calculate_expression(expression) {
        try {
            const response = await fetch(`https://localhost:7273/Calculate?expr=${encodeURIComponent(expression)}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'text/plain'
                }
            });
            if (response.ok) {
                let result = await response.text();
                const integersOnlyCheckbox = document.getElementById('integers-only-checkbox');
                if (integersOnlyCheckbox.checked) {
                    result = Math.floor(parseFloat(result)).toString();
                }
                return result;
            } else {
                handleErrorResponse(response);
                return "Error";
            }
        } catch (error) {
            displayError("Error calculating expression:", error);
            return "Error";
        }
    }

    async function save_result(expression, result) {
        try {
            const exp = `${expression}=${result}`;
            const response = await fetch(`https://localhost:7273/Save?expr=${encodeURIComponent(exp)}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'text/plain'
                }
            });
            if (response.ok) {
                return true;
            } else {
                handleErrorResponse(response);
                return false;
            }
        } catch (error) {
            displayError("Error saving calculation record:", error);
            return false;
        }
    }

    async function loadLastCalculations(count) {
        try {
            const response = await fetch(`https://localhost:7273/LoadLastCalculations?count=${count}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            if (response.ok) {
                const data = await response.json();
                return data.records;
            } else {
                handleErrorResponse(response);
                return [];
            }
        } catch (error) {
            displayError("Error loading last calculations:", error);
            return [];
        }
    }

    function handleErrorResponse(response) {
        switch (response.status) {
            case 400:
                displayError("Invalid input.");
                break;
            case 401:
                displayError("Unauthorized access. Please log in.");
                break;
            case 403:
                displayError("Forbidden. You do not have permission to access this resource.");
                break;
            case 404:
                displayError("Resource not found.");
                break;
            case 500:
                displayError("Internal server error. Please try again later.");
                break;
            case 503:
                displayError("Server is currently unavailable. Please try again later.");
                break;
            default:
                displayError("An unexpected error occurred. Please try again later.");
                break;
        }
    }

    function displayError(...args) {
        console.error(...args);
        let errorContainer = document.getElementById('error-container');
        errorContainer.innerText = args.join(' ');
        errorContainer.style.display = 'block';
    }

    function updateHistory(records) {
        let historyList = document.getElementById('calc-history');
        historyList.innerHTML = ''; // Clear existing history
        records.forEach(record => {
            let newItem = document.createElement('li');
            newItem.className = 'list-group-item';
            newItem.textContent = record;
            historyList.appendChild(newItem);
        });
    }

    let integersOnlyCheckbox = document.getElementById('integers-only-checkbox');
    integersOnlyCheckbox.addEventListener('change', function () {
        let decimalPointButton = document.getElementById('decimal-point-button');
        let calcDisplay = document.getElementById('calc-display');
        if (this.checked) {
            decimalPointButton.disabled = true;
        } else {
            decimalPointButton.disabled = false;
        }
        calcDisplay.value = ""; // Clear the display
    });
});