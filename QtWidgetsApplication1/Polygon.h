#pragma once
#include <cmath>
#include <vector>

class Vector2 {
  double x{0};
  double y{0};

 public:
  Vector2(double x, double y);
  friend class Polygon;

  // 重载乘法
  const Vector2 operator*(const double a);
  friend Vector2 operator*(const double a, const Vector2& b);
  friend const double operator*(const Vector2& a, const Vector2& b);

  // 重载加法
  friend const Vector2 operator+(const Vector2& a, const Vector2& b);

  // 重载加法
  const Vector2 operator-(const Vector2& a);
  friend const Vector2 operator-(const Vector2& a, const Vector2& b);

  // 归一化
  const Vector2 normalize() const;

  // 顺时针旋转90度
  const Vector2 perpendicular() const;
};

bool is_overlap(std::pair<double, double> a, std::pair<double, double> b);

class Polygon {
 public:
  Polygon(std::vector<Vector2> vertices);

  void move(double dx, double dy);
  void move(Vector2 d);

  std::pair<double, double> project(Vector2 axis) const;
  bool is_cover(const Polygon& b);

 private:
  int n;
  std::vector<Vector2> vertices;
  std::vector<Vector2> edges;
};
