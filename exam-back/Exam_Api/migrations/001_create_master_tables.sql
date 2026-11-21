-- Master data: ข้อสอบ และตัวเลือกของแต่ละข้อ
CREATE TABLE tb_m_question (
    id            integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    question_text varchar(500) NOT NULL,
    sort_order    integer      NOT NULL DEFAULT 0,
    is_active     boolean      NOT NULL DEFAULT true,
    create_at     timestamptz  NOT NULL DEFAULT now()
);

CREATE TABLE tb_m_question_choice (
    id          integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    question_id integer      NOT NULL REFERENCES tb_m_question (id) ON DELETE CASCADE,
    choice_text varchar(200) NOT NULL,
    is_correct  boolean      NOT NULL DEFAULT false,
    sort_order  integer      NOT NULL DEFAULT 0
);
CREATE INDEX ix_tb_m_question_choice_question_id ON tb_m_question_choice (question_id);
-- 1 ข้อ มีคำตอบที่ถูกได้เพียงตัวเลือกเดียว
CREATE UNIQUE INDEX ux_tb_m_question_choice_one_correct ON tb_m_question_choice (question_id) WHERE is_correct;
