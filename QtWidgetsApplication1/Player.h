#pragma once
#include <fmt/core.h>

#include <random>
#include <string>
#include <typeinfo>
#include <vector>

#include "Polygon.h"

static int randomint(int min, int max) {
  static std::random_device
      rd;  // Will be used to obtain a seed for the random number engine
  static std::mt19937 gen(
      rd());  // Standard mersenne_twister_engine seeded with rd()
  static std::uniform_int_distribution<int> distrib(min, max);
  return distrib(gen);
}

static double randomdouble(int min, int max) {
  static std::random_device
      rd;  // Will be used to obtain a seed for the random number engine
  static std::mt19937 gen(
      rd());  // Standard mersenne_twister_engine seeded with rd()
  static std::uniform_real_distribution<> distrib(min, max);
  return distrib(gen);
}

struct Player {
  static int PlayerNumber;
  static int PlayerRemainedNumber;

  std::string name{""};
  std::string killed_by{""};

  // �˶�ѧ����
  // �����x��y�Դ������Ͻ�Ϊԭ�㣬����Ϊx������Ϊy
  int width{100};
  int height{15};
  int x{0};  // ���Ͻǵ�x����
  int y{0};  // ���Ͻǵ�y����
  int vx{0};
  int vy{0};
  double speed() const { return sqrt(vx * vx + vy * vy); }
  Polygon polygon;

  // ս����ز���
  int hit_point{510};
  double combat_force_level{0};
  bool is_alive{false};

  // �������
  int creating_order{0};
  int score{0};
  int kill_number{0};
  int survival_rank{0};
  int time_score{0};
  int attack_score{0};
  int bonus{0};
  int survival_time{0};

  // ����
  bool is_player() const { return typeid(this) == typeid(Player); }

  Player(int field_x, int field_y, int height, int width, int creating_order);
  Player(int field_x, int field_y, int height, int width, int creating_order,
         int combat_force_option);
  Player(int field_x, int field_y, int combat_force_option, int creating_order);

  // ���ٶȷ������ƶ�
  // ��������߽���ƶ�����һ��
  void move(int field_x, int field_y);
  // �ٶ��������
  void update_speed();
  // ����һ�����ս����������ε�Ѫ
  void battle(Player& player, int kill_options);
  // ���㣬����һ�����
  virtual void settle(Player& player, int survival_time);
  // �����Ϣ������һ���ַ�������
  std::vector<std::string> info();
};
