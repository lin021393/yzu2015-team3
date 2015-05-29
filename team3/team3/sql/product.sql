-- phpMyAdmin SQL Dump
-- version 3.2.3
-- http://www.phpmyadmin.net
--
-- 主機: localhost
-- 建立日期: May 28, 2015, 12:51 PM
-- 伺服器版本: 5.6.24
-- PHP 版本: 5.2.11

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- 資料庫: `team3`
--

-- --------------------------------------------------------

--
-- 資料表格式： `product`
--

CREATE TABLE IF NOT EXISTS `product` (
  `productid` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `categoryid` int(11) unsigned NOT NULL,
  `productname` varchar(100) COLLATE utf8_unicode_ci DEFAULT NULL,
  `productprice` int(11) unsigned DEFAULT NULL,
  `productimages` varchar(100) COLLATE utf8_unicode_ci DEFAULT NULL,
  `description` text COLLATE utf8_unicode_ci,
  `productremain` int(11) unsigned DEFAULT NULL,
  PRIMARY KEY (`productid`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci AUTO_INCREMENT=54 ;

--
-- 列出以下資料庫的數據： `product`
--

INSERT INTO `product` (`productid`, `categoryid`, `productname`, `productprice`, `productimages`, `description`, `productremain`) VALUES
(1, 3, 'ASUS ZENBOOK UX305', 22900, '123.jpg', '輕盈靈巧的ZenBook UX305沉穩紮實的陶瓷白或黑曜岩外觀色彩、加上同心圓髮絲紋表面處理，無論在遠處欣賞或近距離接觸，都展現出獨一無二的質感；精巧的鑽石切邊，更在細微處流露精品格調。', 50),
(20, 2, 'ASUS LKK', 1999, '234.jpg', '', 99),
(49, 3, 'APPLE Macbook Air', 19999, '456.jpg', 'APPLE 就是潮', 69),
(50, 3, 'APPLE Macbook Air', 19999, '456.jpg', 'APPLE 就是潮', 69),
(51, 3, 'APPLE Macbook Air', 19999, '456.jpg', 'APPLE 就是潮', 69),
(52, 3, 'APPLE Macbook Air', 19999, '456.jpg', 'APPLE 就是潮', 69),
(53, 3, 'APPLE Macbook Air', 19999, '456.jpg', 'APPLE 就是潮', 69);
