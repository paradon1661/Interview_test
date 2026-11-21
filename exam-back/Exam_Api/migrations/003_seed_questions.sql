-- Mock data: ข้อสอบตามแบบ IT 10-1
WITH q AS (
    INSERT INTO tb_m_question (question_text, sort_order) VALUES ('ข้อใดต่างจากข้ออื่น', 1) RETURNING id
)
INSERT INTO tb_m_question_choice (question_id, choice_text, is_correct, sort_order)
SELECT q.id, c.choice_text, c.is_correct, c.sort_order
FROM q, (VALUES ('3', false, 1), ('5', false, 2), ('9', true, 3), ('11', false, 4)) AS c (choice_text, is_correct, sort_order);
-- 9 เป็นตัวเดียวที่ไม่ใช่จำนวนเฉพาะ

WITH q AS (
    INSERT INTO tb_m_question (question_text, sort_order) VALUES ('X + 2 = 4 จงหาค่า X', 2) RETURNING id
)
INSERT INTO tb_m_question_choice (question_id, choice_text, is_correct, sort_order)
SELECT q.id, c.choice_text, c.is_correct, c.sort_order
FROM q, (VALUES ('1', false, 1), ('2', true, 2), ('3', false, 3), ('4', false, 4)) AS c (choice_text, is_correct, sort_order);
