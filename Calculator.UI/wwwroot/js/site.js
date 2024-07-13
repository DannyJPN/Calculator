﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', async function () {
    // Function to read the HTML list of search results and convert it to a JSON string
    console.log("START");

    // Event listener for the Save button
    var digit_buttons = document.getElementsByClassName('btn-input-digit')
    for (var i = 0; i < digit_buttons.length; i++)
    {
        digit_buttons[i].addEventListener('click', function () {
            var caller = this;
            console.log("DIGIT");
            if (caller instanceof HTMLElement)
            {
                var textbox = document.getElementById('calc-display');
                textbox.value += caller.innerText.trim();

            }
        });
    }


    var operator_buttons = document.getElementsByClassName('btn-input-operator')
    const operator_regex = /[+\-*\/]/;
    const dec_dot_regex = /[\.]/;
    const number_regex =  /[0-9]$/
    for (var i = 0; i < operator_buttons.length; i++)
    {
        operator_buttons[i].addEventListener('click', function () {
            var caller = this;
            console.log("OPERATOR");
            if (caller instanceof HTMLElement)
            {
                var textbox = document.getElementById('calc-display');
                if (!operator_regex.test(textbox.value))
                {
                    textbox.value += caller.innerText.trim();
                }
                    
            }
        });
    }


    var decimaldigit_buttons = document.getElementsByClassName('btn-input-decimalpoint')
    for (var i = 0; i < decimaldigit_buttons.length; i++)
    {
        decimaldigit_buttons[i].addEventListener('click', function () {
            var caller = this;
            console.log("DECPOINT");
            if (caller instanceof HTMLElement)
            {
                var textbox = document.getElementById('calc-display');

                var numbers = textbox.value.split(operator_regex)
                var lastnum = numbers[numbers.length-1]
                console.log(lastnum);
                console.log(dec_dot_regex.test(lastnum));
                console.log(number_regex.test(lastnum));
                if (!dec_dot_regex.test(lastnum) && number_regex.test(lastnum))
                {
                    textbox.value += caller.innerText.trim();
                }
            }
        });
    }


    var clear_buttons = document.getElementsByClassName('btn-input-clear')
    for (var i = 0; i < clear_buttons.length; i++) {
        clear_buttons[i].addEventListener('click', function () {
            var caller = this;
         
            if (caller instanceof HTMLElement)
            {
                var textbox = document.getElementById('calc-display');

                textbox.value = "";
                
            }
        });
    }

    var calc_button = document.getElementById('btn-calculate')
    calc_button.addEventListener('click', async function () {
        var caller = this;

        if (caller instanceof HTMLElement)
        {
            var textbox = document.getElementById('calc-display');
            var expression = textbox.value
            var result = await calculate_expression(expression);
            var saving_result = await save_result(expression, result);
            

        }
    });

    async function calculate_expression(expression)
    {
        console.log("EXPR: " + await api_test())


    }
    async function save_result(expression, result)
    {
        console.log("DAVER: " + await api_test())

    }
    async function api_test()
    {
        var fethres = await fetch("https://open.er-api.com/v6/latest/CZK").then(response => response.json()).then(x => x.result)
        return fethres;
    }



    

});
