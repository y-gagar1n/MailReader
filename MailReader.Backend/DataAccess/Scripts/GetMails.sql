USE MailReader;
SELECT Id, Subject, Body, [From]
FROM Mails
ORDER BY Id DESC;