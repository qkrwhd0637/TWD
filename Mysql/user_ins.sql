CREATE DEFINER=`root`@`localhost` PROCEDURE `user_ins`(IN in_id VARCHAR(20), IN in_password VARCHAR(20), OUT out_check INT)
BEGIN
	#존재하는 아이디인지 체크부터 한다.
	SELECT  COUNT(*) INTO out_check
	FROM user_tb a
	WHERE a.id = in_id
	AND (in_id != '' OR in_id IS NOT NULL)
	AND (in_password != '' OR in_password IS NOT NULL);

	#존재하는 아이디가 없다면 추가한다.
	IF out_check = 0
		THEN 
			INSERT INTO user_tb
			VALUES 
			(
				in_id,
				AES_ENCRYPT('4129',SHA2(in_password,512)),
				CURRENT_TIMESTAMP,
				CURRENT_TIMESTAMP,
				CURRENT_TIMESTAMP
			 );
	#존재하는 아이디가 있다면 1을 내보낸다.
	ELSEIF out_check > 0 THEN SET out_check = 1;
	END IF;
END