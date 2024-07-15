// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', async function () {
    // Function to read the HTML list of search results and convert it to a JSON string
    console.log("START");
    const operator_regex = /[+\-*\/]/;
    const dec_dot_regex = /[\.]/;
    const number_regex = /[0-9]$/;
    const records_count = 10;
    // Event listener for the Save button
    var digit_buttons = document.getElementsByClassName('btn-input-digit')
    for (var i = 0; i < digit_buttons.length; i++) {
        digit_buttons[i].addEventListener('click', function () {
            var caller = this;
            console.log("DIGIT");
            if (caller instanceof HTMLElement) {
                var textbox = document.getElementById('calc-display');
                textbox.value += caller.innerText.trim();
            }
        });
    }

    var operator_buttons = document.getElementsByClassName('btn-input-operator')

    for (var i = 0; i < operator_buttons.length; i++) {
        operator_buttons[i].addEventListener('click', function () {
            var caller = this;
            console.log("OPERATOR");
            if (caller instanceof HTMLElement) {
                var textbox = document.getElementById('calc-display');
                if (!operator_regex.test(textbox.value)) {
                    textbox.value += caller.innerText.trim();
                }
            }
        });
    }

    var decimaldigit_buttons = document.getElementsByClassName('btn-input-decimalpoint')
    for (var i = 0; i < decimaldigit_buttons.length; i++) {
        decimaldigit_buttons[i].addEventListener('click', function () {
            var caller = this;
            console.log("DECPOINT");
            if (caller instanceof HTMLElement) {
                var textbox = document.getElementById('calc-display');
                var numbers = textbox.value.split(operator_regex)
                var lastnum = numbers[numbers.length - 1]
                console.log(lastnum);
                console.log(dec_dot_regex.test(lastnum));
                console.log(number_regex.test(lastnum));
                if (!dec_dot_regex.test(lastnum) && number_regex.test(lastnum)) {
                    textbox.value += caller.innerText.trim();
                }
            }
        });
    }

    var clear_buttons = document.getElementsByClassName('btn-input-clear')
    for (var i = 0; i < clear_buttons.length; i++) {
        clear_buttons[i].addEventListener('click', function () {
            var caller = this;
            if (caller instanceof HTMLElement) {
                var textbox = document.getElementById('calc-display');
                textbox.value = "";
            }
        });
    }

    var calc_button = document.getElementById('btn-calculate')
    calc_button.addEventListener('click', async function () {
        var caller = this;
        if (caller instanceof HTMLElement) {
            var textbox = document.getElementById('calc-display');
            var expression = textbox.value;
            var result = await calculate_expression(expression);
            if (result !== "Error") {
                var saving_result = await save_result(expression, result);
                if (saving_result) {
                    textbox.value = result;
                    var records = await loadLastCalculations(records_count);
                    updateHistory(records);
                } else {
                    alert("Failed to save the calculation record.");
                }
            } else {
                alert("Invalid expression.");
            }
        }
    });

    async function calculate_expression(expression)
    {
        try
        {
            const response = await fetch(`https://localhost:7273/Calculate?expr=${encodeURIComponent(expression)}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'text/plain'
                }
            });
            console.log(response);
            if (response.ok)
            {
                const result = await response.text();
                console.log("RESULT: " + expression + " = " + integersOnlyCheckbox.checked?round(result) :result);
                return result.toString();
            }
            else
            {
                 console.error("Error calculating expression:", response.statusText);
                 return "Error";
            }
        }
        catch (error)
        {
                console.error("Error calculating expression:", error);
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
            return response.ok;
        } catch (error) {
            console.error("Error saving calculation record:", error);
            return false;
        }
    }



    // Function to load the last "count" calculations
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
                console.error("Error loading last calculations:", response.statusText);
                return [];
            }
        } catch (error) {
            console.error("Error loading last calculations:", error);
            return [];
        }
    }

    // Function to update the history section with the retrieved records
    function updateHistory(records) {
        var historyList = document.getElementById('calc-history');
        historyList.innerHTML = ''; // Clear existing history
        records.forEach(record => {
            var newItem = document.createElement('li');
            newItem.className = 'list-group-item';
            newItem.textContent = record;
            historyList.appendChild(newItem);
        });
    }

    // Event listener for the 'integers-only-checkbox'
    var integersOnlyCheckbox = document.getElementById('integers-only-checkbox');
    integersOnlyCheckbox.addEventListener('change', function () {
        var decimalPointButton = document.getElementById('decimal-point-button');
        var calcDisplay = document.getElementById('calc-display');
        if (this.checked) {
            decimalPointButton.disabled = true;
        } else {
            decimalPointButton.disabled = false;
        }
        calcDisplay.value = ""; // Clear the display
    });

});