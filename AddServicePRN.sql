SET IDENTITY_INSERT Service ON;
INSERT INTO Service (ID, Name, Description, Duration, Cost, Rank)
VALUES 
(1,N'khám tổng quát', N'khám tổng quát giá theo 1 lần', 1, 100000, 1),
(2,N'nhổ răng thường', N'nhổ răng thường giá theo 1 răng', 2, 200000, 2),
(3,N'nhổ răng khôn', N'nhổ răng khôn giá theo 1 răng', 3, 1000000, 4),
(4,N'tẩy trắng răng', N'tẩy trắng răng giá theo 1 lần', 1, 400000, 2);

SET IDENTITY_INSERT Service OFF;
-- Kiểm tra dữ liệu đã được thêm vào bảng Service hay chưa
SELECT * FROM Service;