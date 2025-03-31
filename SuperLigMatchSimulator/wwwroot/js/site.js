


	// Standings tablosunu güncelle
	function updateStandingsTable() {
			const standings = JSON.parse(localStorage.getItem('standings')).map(item => {
	   const convertedItem = {};
	   for (const key in item) {
		   const lowercaseKey = key.charAt(0).toLowerCase() + key.slice(1);
		   convertedItem[lowercaseKey] = item[key];
	   }
	   return convertedItem;
	});;
		const tbody = document.getElementById('standingsTableBody');
		tbody.innerHTML = '';
		console.log(standings[0])
		standings.forEach((team, index) => {
			let rowColor = '';
			if (index === 0) rowColor = 'bg-green-300 text-black';
			else if (index === 1) rowColor = 'bg-green-100 text-black';
			else if (index === 2) rowColor = 'bg-blue-200 text-black';
			else if (index === 3) rowColor = 'bg-yellow-200 text-black';
			else if (index >= standings.length - 4) rowColor = 'bg-red-200 text-black';

			const row = `
				<tr class="${rowColor} hover:opacity-90 transition duration-200">
					<td class="px-2 py-0.5 text-xs text-sm font-medium">
						<div class="flex items-center space-x-1">
							<span>${getTrendIcon(team.trendDirection)}</span>
							<span>${index + 1}</span>
						</div>
					</td>
					<td class="px-2 py-0.5 text-xs">
						<div class="flex items-center">
							<div class="flex-shrink-0 h-10 w-10 mr-3">
								<img class="h-10 w-10 rounded-full object-cover" src="${team.teamLogo}" alt="${team.team} Logo">
							</div>
							<div class="text-sm font-medium text-gray-900">${team.team}</div>
							${team.reductedPoints !== 0 ? `<div class="text-xs text-gray-500 mt-1">(${team.reductedPoints} puan)</div>` : ''}
						</div>
					</td>
					<td class="px-2 py-0.5 text-xs text-center text-sm text-gray-500">${team.played}</td>
					<td class="px-2 py-0.5 text-xs text-center text-sm text-green-600">${team.won}</td>
					<td class="px-2 py-0.5 text-xs text-center text-sm text-yellow-600">${team.drawn}</td>
					<td class="px-2 py-0.5 text-xs text-center text-sm text-red-600">${team.lost}</td>
					<td class="px-2 py-0.5 text-xs text-center text-sm text-blue-600">${team.goalsFor}</td>
					<td class="px-2 py-0.5 text-xs text-center text-sm text-red-600">${team.goalsAgainst}</td>
					<td class="px-2 py-0.5 text-xs text-center text-sm ${team.goalDifference >= 0 ? 'text-green-600' : 'text-red-600'}">${team.goalDifference}</td>
					<td class="px-2 py-0.5 text-xs text-center text-sm font-semibold text-gray-900">${team.points}</td>
				</tr>
			`;
			tbody.innerHTML += row;
		});
	}

	function getTrendIcon(trend) {
		switch(trend) {
			case 0: return '<span class="text-green-600">↑</span>';
			case 1: return '<span class="text-red-600">↓</span>';
			default: return '<span class="text-gray-500">→</span>';
		}
	}




	document.addEventListener('DOMContentLoaded', function() {
		console.log("updated");
		updateStandingsTable();

	});

function resetAllPredictions() {
	fetch('/Match/ResetAllPredictions', {
		method: 'GET',
	})
		.then(response => {
			if (!response.ok) {
				return response.json().then(err => {
					console.error('Server error:', err);
					throw new Error(err.details || 'Server error occurred');
				});
			}
			return response.json();
		})
		.then(data => {
			console.log('Received data:', data);
			localStorage.setItem('standings', JSON.stringify(data.standings));
			localStorage.setItem('matches', JSON.stringify(data.matches));
			updateStandingsTable();
			updateMatchesDisplay();
			location.reload();
		})
		.catch(error => {
			console.error('Error:', error);
			// Hata durumunda kullanıcıya bilgi ver
			alert('Bir hata oluştu: ' + error.message);
		});
}