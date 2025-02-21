CREATE USER 'api'@'%' IDENTIFIED WITH authentication_plugin BY 'APIPass!';
GRANT ALL PRIVILEGES ON *.* TO 'api'@'%' WITH GRANT OPTION;