CREATE DEFINER=`root`@`localhost` PROCEDURE `userinfo_sel`(in_id VARCHAR(20), in_password VARCHAR(20))
BEGIN

	SELECT  a.id, b.last_round, b.top_round, access_date
	FROM user_tb a
    LEFT JOIN play_tb b
    ON a.id = b.id
	WHERE a.id = in_id
    AND a.password = AES_ENCRYPT('4129',SHA2(in_password,512))
	AND (in_id != '' OR in_id IS NOT NULL)
	AND (in_password != '' OR in_password IS NOT NULL);

END