USE PayrollSystem

----- Life Insurance API -----
INSERT INTO APIs(Name, URL, Token, SecurityKeyName, EndpointMethod)
VALUES('Life Insurance', 
		'https://poliza-friends-grg0h9g5crf2hwh8.southcentralus-01.azurewebsites.net/api/LifeInsurance',
		'1D0B194488C091852597B9AF7D1AA8F23D55C9784815489CF0A488B6F2C6D5C4C569AD51231FACB9920E5A763FE388247E03131D1601AC234E86BC0D266EB6A7',
		'FRIENDS-API-TOKEN',
		 'GET')

----- Association API -----
INSERT INTO APIs(Name, URL, Token, SecurityKeyName, EndpointMethod)
VALUES('Association', 
		'https://asociacion-geems-c3dfavfsapguhxbp.southcentralus-01.azurewebsites.net/api/public/calculator/calculate',
		'Tralalerotralala',
		'API-KEY',
		 'POST')

----- Medicare API -----
INSERT INTO APIs(Name, URL, Token, SecurityKeyName, EndpointMethod)
VALUES('Medicare', 
		'https://mediseguro-vorlagenersteller-d4hmbvf7frg7aqan.southcentralus-01.azurewebsites.net/api/MediSeguroMonto',
		'TOKEN123',
		'token',
		 'POST')
