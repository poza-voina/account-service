CREATE OR REPLACE PROCEDURE accrue_interest(p_account_id UUID)
LANGUAGE plpgsql
AS $$
DECLARE
    v_rate NUMERIC;
    v_balance NUMERIC;
    v_interest NUMERIC;
    v_currency TEXT;
BEGIN
    SELECT "interestRate", "balance", "currency"
    INTO v_rate, v_balance, v_currency
    FROM accounts
    WHERE id = p_account_id
    FOR UPDATE;

    IF v_rate IS NULL THEN
        RAISE EXCEPTION 'Счет % не найден или ключевая ставка = NULL', p_account_id;
    END IF;

    v_interest := v_balance * v_rate;

    UPDATE accounts
    SET balance = balance + v_interest
    WHERE id = p_account_id;

    INSERT INTO transactions("bankAccountId", currency, amount, "type", "createdAt", "description", "isApply")
    VALUES (p_account_id, v_currency, v_interest, 'Debit', NOW(), 'add dengi', TRUE);
END;
$$;