-- phpMyAdmin SQL Dump
-- version 3.2.3
-- http://www.phpmyadmin.net
--
-- �D��: localhost
-- �إߤ��: May 28, 2015, 12:51 PM
-- ���A������: 5.6.24
-- PHP ����: 5.2.11

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- ��Ʈw: `team3`
--

-- --------------------------------------------------------

--
-- ��ƪ�榡�G `product`
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
-- �C�X�H�U��Ʈw���ƾڡG `product`
--

INSERT INTO `product` (`productid`, `categoryid`, `productname`, `productprice`, `productimages`, `description`, `productremain`) VALUES
(1, 3, 'ASUS ZENBOOK UX305', 22900, '123.jpg', '�����F����ZenBook UX305�Ií�Ϲꪺ�����թζ��`���~�[��m�B�[�W�P�߶�v�������B�z�A�L�צb���B�Y��Ϊ�Z����Ĳ�A���i�{�X�W�@�L�G����P�F�륩���p�ۤ���A��b�ӷL�B�y�S��~��աC', 50),
(20, 2, 'ASUS LKK', 1999, '234.jpg', '', 99),
(49, 3, 'APPLE Macbook Air', 19999, '456.jpg', 'APPLE �N�O��', 69),
(50, 3, 'APPLE Macbook Air', 19999, '456.jpg', 'APPLE �N�O��', 69),
(51, 3, 'APPLE Macbook Air', 19999, '456.jpg', 'APPLE �N�O��', 69),
(52, 3, 'APPLE Macbook Air', 19999, '456.jpg', 'APPLE �N�O��', 69),
(53, 3, 'APPLE Macbook Air', 19999, '456.jpg', 'APPLE �N�O��', 69);
