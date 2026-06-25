create database pharmacy_webdev;
create user 'webdev_pharmacy_user'@'%' identified by 'webdev_pharmacy_user';
grant create,drop,alter,insert,update,select,delete on pharmacy_webdev.* to webdev_pharmacy_user;
grant index on pharmacy_webdev.* to webdev_pharmacy_user;

