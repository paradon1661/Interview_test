-- ผลสอบ 1 แถว ต่อการส่งข้อสอบ 1 ครั้ง
CREATE TABLE tb_exam_result (
    id              uuid         PRIMARY KEY,
    full_name       varchar(200) NOT NULL,
    score           integer      NOT NULL CHECK (score >= 0),
    total_questions integer      NOT NULL CHECK (total_questions >= 0),
    create_at       timestamptz  NOT NULL DEFAULT now(),
    CONSTRAINT ck_tb_exam_result_score CHECK (score <= total_questions)
);
CREATE INDEX ix_tb_exam_result_create_at ON tb_exam_result (create_at DESC);

-- คำตอบที่ผู้สอบเลือก: 1 ข้อ ตอบได้ 1 คำตอบ
CREATE TABLE tb_exam_answer (
    id             uuid    PRIMARY KEY,
    exam_result_id uuid    NOT NULL REFERENCES tb_exam_result (id) ON DELETE CASCADE,
    question_id    integer NOT NULL REFERENCES tb_m_question (id),
    choice_id      integer NOT NULL REFERENCES tb_m_question_choice (id),
    is_correct     boolean NOT NULL,
    CONSTRAINT ux_tb_exam_answer_one_per_question UNIQUE (exam_result_id, question_id)
);
