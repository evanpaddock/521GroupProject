// Declare a global variable to store the workout plan data
var globalWorkoutPlanData = null;

document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('workoutPlanForm');

    form.addEventListener('submit', function (event) {
        event.preventDefault();
        const submitBtn = document.getElementById('submitBtn');
        submitBtn.disabled = true;

        const workoutData = {
            gender: document.getElementById('gender')
                ? document.getElementById('gender').value
                : '',
            height: document.getElementById('height')
                ? document.getElementById('height').value
                : '',
            weight: document.getElementById('weight')
                ? document.getElementById('weight').value
                : '',
            age: document.getElementById('age')
                ? document.getElementById('age').value
                : '',
            fitnessLevel: document.getElementById('fitnessLevel')
                ? document.getElementById('fitnessLevel').value
                : '',
            days: Array.from(document.querySelectorAll('input[name="days"]:checked'))
                .map((el) => el.value)
                .join(', '),
            timePerSession: document.getElementById('timePerSession')
                ? document.getElementById('timePerSession').value
                : '',
            goal: document.getElementById('goal')
                ? document.getElementById('goal').value
                : '',
        };

        const formData = new FormData();
        Object.entries(workoutData).forEach(([key, value]) => {
            formData.append(key, value);
        });

        let app = document.getElementById('app');
        app.innerHTML = `<div id="loading"><p>Generating custom workout plan...</p><div class="loader"></div></div>`;

        fetch('https://ironparadise.azurewebsites.net/GPT/CreateWorkoutPlan', {
            method: 'POST',
            headers: {
                'Accept': 'application/json', // Expecting JSON in response
            },
            body: formData
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json(); // Convert response to JSON
            })
            .then(responseJson => {
                app.innerHTML = responseJson.html; // Set the HTML content
                globalWorkoutPlanData = responseJson.data; // Store the data
                // Re-enable the submit button
                if (document.getElementById('submitBtn')) {
                    document.getElementById('submitBtn').disabled = false;
                }
            })
            .catch(error => {
                console.error('Error:', error);
                app.innerHTML = '<p>Error loading results.</p>';

                // Re-enable the submit button
                if (document.getElementById('submitBtn')) {
                    document.getElementById('submitBtn').disabled = false;
                }
            });
    });
});

const generatePDF = async () => {
    const doc = new window.jspdf.jsPDF();
    let startY = 10;

    // Function to add a header
    const addHeader = () => {
        doc.setFontSize(14); // Set the font size for the header
        doc.text('Here is your plan. Thanks for using Iron Paradise!', 10, startY);
        startY += 15; // Increase startY to add padding below the header
    };

    // Add header for the first page
    addHeader();

    globalWorkoutPlanData.forEach((dayPlan) => {
        // Check if we need a new page
        if (startY >= doc.internal.pageSize.height - 20) {
            doc.addPage();
            startY = 10; // Reset startY for the new page
            addHeader(); // Add header for the new page
        }

        // Add Day name
        doc.setFontSize(12); // Set the font size for the day name
        doc.text(dayPlan.dayName, 10, startY);

        // Increment startY for the table
        startY += 10;

        // Create the table under the Day name
        const tableColumnNames = ["Exercise", "Sets", "Reps"];
        const tableRows = dayPlan.exercises.map(exercise => [
            exercise.item1, // Exercise name
            exercise.item2.toString(), // Sets
            exercise.item3.toString() // Reps
        ]);

        // Draw table
        doc.autoTable(tableColumnNames, tableRows, {
            startY: startY,
            margin: { top: 20, bottom: 30, right: 10, left: 10 },
            theme: 'grid',
            didDrawPage: function (data) {
                // Update startY for the next section after the table
                startY = data.cursor.y + 20;
                // Check if we've drawn to the end of the page
                if (startY >= doc.internal.pageSize.height - 20) {
                    doc.addPage();
                    startY = 10; // Reset startY for the new page
                    addHeader(); // Add header for the new page
                }
            }
        });

        // Update startY for the next day's content
        startY = doc.autoTable.previous.finalY + 10;
    });

    // Save the created PDF
    doc.save('workout-plan.pdf');
};
